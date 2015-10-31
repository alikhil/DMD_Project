using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.DynamicData;
using Npgsql;
using NpgsqlTypes;
using Project_DMD.Attributes;

namespace Project_DMD.Classes
{
    public class AutoSqlGenerator
    {
        private static AutoSqlGenerator _instance;

        public static AutoSqlGenerator Instance
        {
            get { return _instance ?? (_instance = new AutoSqlGenerator()); }
        }

        private AutoSqlGenerator()
        {
        }
        public static class Constants
        {
            public readonly static string ConnectionString = "Host=localhost;Username=postgres;Password=postgres;Database=PMS;";

            public readonly static string AppUsersTableName = "Client";

            /// <summary>
            /// Template string in order to drop table.
            /// <para>You should use String.Formater, where </para>
            /// <para>{0} - Table Name.</para>
            /// </summary>
            public readonly static string DropTableTemplate = "DROP TABLE {0};";

            /// <summary>
            /// Template string for insertion into POSTGRESQL. 
            /// <para>You should use String.Formater, where </para>
            /// <para>{0} - Table Name </para>
            /// <para>{1} - Ordered Name Columns (For Example: Column1, Column2) </para>
            /// <para>{2} - Ordered Values For Columns (For Example: (Column1Value, Column2Value) ) </para>
            /// <para>{3} - Column Name of returning value. </para>
            /// </summary>
            public static readonly string InsertTableTemplate = "INSERT INTO {0} ({1}) VALUES {2} RETURNING {3};";

            /// <summary>
            /// Template string for insertion into POSTGRESQL. 
            /// <para>You should use String.Formater, where </para>
            /// <para>{0} - Table Name </para>
            /// <para>{1} - Ordered Values For Columns (For Example: (Column1Value, Column2Value) ) </para>
            /// <para>{2} - Column Name of returning value. </para>
            /// </summary>
            public static readonly string InsertTableTemplateWithoutColumns = "INSERT INTO {0} VALUES {1} RETURNING {2};";

            /// <summary>
            /// Template string for selection.
            /// <para>You should use String.Formater, where</para>
            /// <para>{0} - From what table we select (TableName)</para>
            /// <para>{1} - Selection Filters (ColumnName = SomeValue or ColumnName1 = SomeValue1...).</para>
            /// </summary>
            public static readonly string SelectFromTableWhereTemplate = "SELECT * FROM {0} WHERE {1};";

            /// <summary>
            /// Template string for selection.
            /// <para>You should use String.Formater, where</para>
            /// <para>{0} - From what table we select (TableName).</para>
            /// </summary>
            public static readonly string SelectAllFromTableTemplate = "SELECT * FROM {0};";

            /// <summary>
            /// Template string for update.
            /// <para>You should use String.Formater, where</para>
            /// <para>{0} - What table we refresh (TableName)</para>
            /// <para>{1} - Refresh expression (i.e Views = 0)</para>
            /// <para>{2} - Update filter, on what records refresh expression will be applied</para>.
            /// </summary>
            public static readonly string UpdateOnTemplate = "Update {0} SET {1} WHERE {2};";
        }

        public Dictionary<string,string> ExecuteCommand(string command)
        {
            using (var connection = new NpgsqlConnection(Constants.ConnectionString))
            {
                connection.Open();
                
                var query = new NpgsqlCommand(command, connection);
                Console.WriteLine(query.CommandText);
                var reader = query.ExecuteReader();
                var dictionary = new Dictionary<string, string>();
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        dictionary[reader.GetName(i)] = reader[i].ToString();
                    }
                }
                return dictionary;
            }
        }

        public IEnumerable<Dictionary<string, string>> LazyExecute(string command)
        {
            using (var connection = new NpgsqlConnection(Constants.ConnectionString))
            {
                connection.Open();

                var query = new NpgsqlCommand(command, connection);
                Console.WriteLine(query.CommandText);
                var reader = query.ExecuteReader();
                while (reader.Read())
                {
                    var dictionary = new Dictionary<string, string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        dictionary[reader.GetName(i)] = reader[i].ToString();
                    }
                   yield return     dictionary;
                }
            }
        }

        public List<Dictionary<string, string>> ExecuteCommandReturnList(string command)
        {
            var result = new List<Dictionary<string, string>>();
            using (var connection = new NpgsqlConnection(Constants.ConnectionString))
            {
                connection.Open();

                var query = new NpgsqlCommand(command, connection);
                Console.WriteLine(query.CommandText);
                var reader = query.ExecuteReader();
                while (reader.Read())
                {
                    var dictionary = new Dictionary<string, string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        dictionary[reader.GetName(i)] = reader[i].ToString();
                    }
                    result.Add(dictionary);
                }
                return result;
            }
        }

        public void Update(object entity, string primaryKey)
        {
            var tableName = GetTableName(entity);
            string primaryProperty;
            List<object> values;
            var columns = GetColumnsAndValues(entity, out values, out primaryProperty, true);
            using (var connection = new NpgsqlConnection(Constants.ConnectionString))
            {
                connection.Open();
                var command = new NpgsqlCommand(String.Format(Constants.UpdateOnTemplate,tableName,
                    String.Join(",", columns.Zip(values,(column, value) => column + "=" + value)), 
                    primaryProperty + "=" + primaryKey), connection);
                Console.WriteLine(command.CommandText);
                command.ExecuteScalar();
            }

        }

        public string Add(object entity)
        {
            var tableName = GetTableName(entity);

            string primaryProperty;
            List<object> values;
            var columns = GetColumnsAndValues(entity, out values, out primaryProperty);
            using (var conn = new NpgsqlConnection(Constants.ConnectionString))
            {
                conn.Open();

                NpgsqlCommand query = new NpgsqlCommand(
                    String.Format(Constants.InsertTableTemplate, tableName,
                        String.Join(",", columns),
                        '(' + String.Join(",", columns.Select(x => "@" + x)) + ')', primaryProperty), conn);

                var i = 0;
                foreach (var value in values)
                {
                        query.Parameters.AddWithValue("@" + columns[i++], value);
                }
                Console.WriteLine(query.CommandText);

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

            using (var conn = new NpgsqlConnection(Constants.ConnectionString))
            {
                conn.Open();
                var query = new NpgsqlCommand(String.Format(Constants.SelectFromTableWhereTemplate, tableName,
                    primaryProperty + "=" + primaryKey), conn);
                //query.Parameters.AddWithValue("@" + primaryProperty, "'" + primaryKey + "'");
                Console.WriteLine(query.CommandText);

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
            using (var connection = new NpgsqlConnection(Constants.ConnectionString))
            {
                connection.Open();
                NpgsqlCommand sqlQuery;
                if (query != null && query.Count > 0)
                {
                    sqlQuery = new NpgsqlCommand(
                        String.Format(Constants.SelectFromTableWhereTemplate, tableName, String.Join(" AND ",
                            query.Select(pair => pair.Key + "=" + pair.Value))), connection);
                   // query.ForEach(pair => sqlQuery.Parameters.AddWithValue(pair.Key, pair.Value));
                }
                else
                    sqlQuery = new NpgsqlCommand(
                        String.Format(Constants.SelectAllFromTableTemplate, tableName), connection);
                Console.WriteLine(sqlQuery.CommandText);
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
                var _primaryAttribute = propertyInfo.GetCustomAttribute(typeof (AgsPrimary));
                if (_primaryAttribute == null) continue;
                var primaryAttribute = (AgsPrimary) _primaryAttribute;
                primaryProperty = primaryAttribute.Name ?? propertyInfo.Name;
                break;
            }
            return primaryProperty;
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
                    propertyInfo.SetValue(entity, Convert.ChangeType(dictionary[fieldName],propertyInfo.PropertyType));
            }
            return entity;
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
                        values.Add(format ? propValue.ToString().PutIntoQuotes() : propValue.ToString());
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
    }

    public static class StringExtensions
    {
        public static string PutIntoQuotes(this string value)
        {
            return "'" + value + "'";
        }
    }
}