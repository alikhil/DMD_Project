using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.DynamicData;
using Npgsql;
using NpgsqlTypes;
using Project_DMD.Attributes;
using Project_DMD.Models;

namespace Project_DMD.Classes
{
    public class AutoSqlGenerator : DatabaseProvider
    {
        private static AutoSqlGenerator _instance;

        public static AutoSqlGenerator Instance
        {
            get { return _instance ?? (_instance = new AutoSqlGenerator()); }
        }

        private AutoSqlGenerator()
        {
        }
        
        

        public void Update(object entity, string primaryKey)
        {
            var tableName = GetTableName(entity);
            string primaryProperty;
            List<object> values;
            var columns = GetColumnsAndValues(entity, out values, out primaryProperty, true);
            using (var connection = CreateConnection())
            {
                var command = new NpgsqlCommand(String.Format(DatabaseConstants.UpdateOnTemplate,tableName,
                    String.Join(",", columns.Zip(values,(column, value) => column + "=" + value)), 
                    primaryProperty + "=" + primaryKey), connection);
                Log(command.CommandText);
                command.ExecuteScalar();
            }

        }

        public string Add(object entity)
        {
            var tableName = GetTableName(entity);

            string primaryProperty;
            List<object> values;
            var columns = GetColumnsAndValues(entity, out values, out primaryProperty);
            using (var conn = CreateConnection())
            {
                NpgsqlCommand query = new NpgsqlCommand(
                    String.Format(DatabaseConstants.InsertTableTemplate, tableName,
                        String.Join(",", columns),
                        '(' + String.Join(",", columns.Select(x => "@" + x)) + ')', primaryProperty), conn);

                var i = 0;
                foreach (var value in values)
                {
                        query.Parameters.AddWithValue("@" + columns[i++], value);
                }
                Log(query.CommandText);

                var result = query.ExecuteScalar();
                return result.ToString();
            }
        }

        public T Get<T>(object primaryKey) where T : new()
        {
            T entity = new T();
            Type type = entity.GetType();

            var tableName = GetTableName(entity);
            var primaryProperty = GetPrimaryProperty<T>(type);

            if (primaryProperty == "")
                throw new ArgumentException("Model does not have primary property");

            using (var conn = CreateConnection())
            {
                var query = new NpgsqlCommand(String.Format(DatabaseConstants.SelectFromTableWhereTemplate, tableName,
                    primaryProperty + "=" + primaryKey), conn);
                //query.Parameters.AddWithValue("@" + primaryProperty, "'" + primaryKey + "'");
                Log(query.CommandText);

                var reader = query.ExecuteReader();

                var dictionary = new Dictionary<string, string>();
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        dictionary[reader.GetName(i)] = reader[i].ToString();
                    }
                }
                entity = ParseDictionary<T>(dictionary);
                return entity;
            }
        }

        /// <summary>
        /// Searching for rows in db with given pair of key and value
        /// </summary>
        /// <typeparam name="T">type of entity</typeparam>
        /// <param name="query">list of key and value</param>
        /// <returns>list of entity which matchs query</returns>
        public List<T> FindAll<T>(Dictionary<string, string> query) where T : new()
        {
            var result = new List<T>();
            var tableName = GetTableName(new T());
            using (var connection = CreateConnection())
            {
                NpgsqlCommand sqlQuery;
                if (query != null && query.Count > 0)
                {
                    sqlQuery = new NpgsqlCommand(
                        String.Format(DatabaseConstants.SelectFromTableWhereTemplate, tableName, String.Join(" AND ",
                            query.Select(pair => pair.Key + "=" + pair.Value.PutIntoDollar()))), connection);
                   // query.ForEach(pair => sqlQuery.Parameters.AddWithValue(pair.Key, pair.Value));
                }
                else
                    sqlQuery = new NpgsqlCommand(
                        String.Format(DatabaseConstants.SelectAllFromTableTemplate, tableName), connection);
                Log(sqlQuery.CommandText);
                var reader = sqlQuery.ExecuteReader();
                while (reader.Read())
                {
                    var dictionary = new Dictionary<string,string>();

                    for(int i = 0; i < reader.FieldCount;i++)
                        dictionary.Add(reader.GetName(i), reader[i].ToString());
                    result.Add(ParseDictionary<T>(dictionary));
                }
            }
            return result;
        }
        
        public T ParseDictionary<T>(Dictionary<string, string> dictionary) where T : new()
        {
            if (dictionary == null || dictionary.Count == 0)
                return default(T);
            var entity = new T();
            var type = entity.GetType();
            foreach (var propertyInfo in type.GetProperties())
            {
                var attribute = propertyInfo.GetCustomAttribute<AgsAttribute>();
                if(attribute == null)
                    continue;
                
                var fieldName = (attribute.Name ?? propertyInfo.Name).ToLower();
                if(dictionary.ContainsKey(fieldName) && !string.IsNullOrEmpty(dictionary[fieldName]))
                    propertyInfo.SetValue(entity, СhangeType(dictionary[fieldName],propertyInfo.PropertyType));
            }
            return entity;
        }

        #region helper methods
        private object СhangeType(object ob, Type t)
        {
            if (t.IsEnum)
                return Enum.Parse(t, ob.ToString());
            return Convert.ChangeType(ob, t);
        }
        private static List<string> GetColumnsAndValues(object entity, out List<object> values, out string primaryProperty, bool format = false)
        {
            Type typeOfObject = entity.GetType();
            var columns = new List<string>();
            values = new List<object>();
            primaryProperty = "1";
            foreach (var propertyInfo in typeOfObject.GetProperties())
            {
                foreach (var customAttribute in propertyInfo.GetCustomAttributes(typeof (AgsAttribute)))
                {
                    var agsAttribute = (AgsAttribute) customAttribute;
                    var agsPrimary = agsAttribute as AgsPrimary;
                    if (agsPrimary != null)
                    {
                        primaryProperty = agsPrimary.Name ?? propertyInfo.Name;
                        if(!agsPrimary.Insert)
                        continue;
                    }

                    var columnName = agsAttribute.Name ?? propertyInfo.Name;

                    columns.Add(columnName);

                    var propValue = propertyInfo.GetValue(entity);
                    if (propValue is string && !agsAttribute.Int)
                        values.Add(format ? propValue.ToString().PutIntoDollar() : propValue.ToString());
                    else if (propValue is DateTime)
                    {
                        var dt = Convert.ToDateTime(propValue.ToString());
                        values.Add(dt);
                    }
                    else
                    {
                        values.Add(agsAttribute.Int ? Convert.ToInt32(propValue) : propValue);
                    }
                }
            }
            return columns;
        }

        private static string GetTableName(object entity)
        {
            var typeOfObject = entity.GetType();
            var model = typeOfObject.GetCustomAttribute<AgsModel>();
            if (model == null)
                throw new ArgumentException("Object must be taged by AgsModel attribute", "entity");

            var tableName = model.TableName ?? typeOfObject.Name;
            return tableName;
        }

        private string GetPrimaryProperty<T>(Type type) where T : new()
        {
            string primaryProperty = "";
            foreach (var propertyInfo in type.GetProperties())
            {
                var _primaryAttribute = propertyInfo.GetCustomAttribute(typeof(AgsPrimary));
                if (_primaryAttribute == null) continue;
                var primaryAttribute = (AgsPrimary)_primaryAttribute;
                primaryProperty = primaryAttribute.Name ?? propertyInfo.Name;
                break;
            }
            return primaryProperty;
        }
        #endregion
    }

    public static class StringExtensions
    {
        public static string PutIntoQuotes(this string value)
        {
            return "'" + value + "'";
        }

        public static string PutIntoDollar(this string value)
        {
            string alphabet = "qwertyuiopasdfghjklzxvbnmcQWERTYUIOPASDFGHJKLZXVBNMC";
            var random = new Random();
            int k = 3;
            var key = "$";
            for (var i = 0; i < k; i++)
                key += alphabet[random.Next(alphabet.Length)];
            key += '$';
            return key + value + key;
        }
    }
}