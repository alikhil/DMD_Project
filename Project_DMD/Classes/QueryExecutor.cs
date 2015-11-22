using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using Project_DMD.Models;
using WebGrease.Css.Extensions;
using Npgsql;
using Project_DMD.Classes.Extensions;

namespace Project_DMD.Classes
{
    public sealed class QueryExecutor : DatabaseProvider
    {
        static readonly QueryExecutor _instance = new QueryExecutor();

   
        public static QueryExecutor Instance
        {
	        get { return _instance; }
        }


        private QueryExecutor()
        {

        }

        #region Base Methods

        public T ExecuteCommandScalar<T>(string command)
        {
            using (var connection = CreateConnection())
            {

                var query = new NpgsqlCommand(command, connection);
                Log(query.CommandText);
                var obj = query.ExecuteScalar();
                
                return (T)obj;
            }
        }
        public Dictionary<string, string> ExecuteCommand(string command)
        {
            using (var connection = CreateConnection())
            {

                var query = new NpgsqlCommand(command, connection);
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
                return dictionary;
            }
        }

        public List<Dictionary<string, string>> ExecuteCommandReturnList(string command)
        {
            var result = new List<Dictionary<string, string>>();
            using (var connection = CreateConnection())
            {

                var query = new NpgsqlCommand(command, connection);
                query.Prepare();
                Log(query.CommandText);
                var reader = query.ExecuteReader();
               
                return ReadResultSet(reader);
            }
        }
        private List<Dictionary<string, string>> ReadResultSet(NpgsqlDataReader reader)
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
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

        
        #endregion

        public Article GetArticleById(int id)
        {
            var commandGetArticle = "SELECT * FROM Article WHERE Article.ArticleID = " + id.ToString() + " LIMIT 1;";
            var articleData = ExecuteCommand(commandGetArticle);

            if(articleData.Count == 0)
                return null;

            var authors = GetAuthorsByArticleID(id);
            var categories = GetCategoriesByArticleID(id);

            return new Article()
                .WithId(Convert.ToInt32(articleData["articleid"]))
                .WithTitle(articleData["title"])
                .WithSummary(articleData["summary"])
                .WithPublished(ParseDateTime(articleData["published"]))
                .WithUpdate(ParseDateTime(articleData["updated"]))
                .WithViews(Convert.ToInt32(articleData["views"]))
                .WithDoi(articleData["doi"])
                .WithJournalReference(articleData["journalreference"])
                .WithAuthors(authors)
                .WithCategories(categories)
                .WithUrl(articleData["url"]);
        }

        public int AddArticle(Article article)
        {
            article.WithPublished(DateTime.Now);

            string query = DatabaseConstants.InsertTableTemplateWithoutColumns;
            query = String.Format(query, "Article",  article.ToSql(), "ArticleID");

            var queryData = ExecuteCommand(query);
            
            if (queryData == null)
                throw new InvalidDataException("Given article is not valid. (ID isn't presented in table, invalid date and so on)");

            article.WithId(Convert.ToInt32(queryData["articleid"]));

            AddArticleAuthors(article);
            AddArticleCategories(article);

            return article.ArticleId;
        }

        public bool UpdateArticle(Article article)
        {
            Article oldArticle = GetArticleById(article.ArticleId);

            article.WithUpdate(DateTime.Now)
                .WithPublished(oldArticle.Published);

            var query = DatabaseConstants.UpdateOnTemplate;
            var values = "(ArticleID, Title, Summary, Published, Updated, Views, URL, DOI, JournalReference) = " +
                         article.ToSql();

            query = String.Format(query, "Article", values, "ArticleID = " + article.ArticleId.ToString());
            ExecuteCommand(query);

            RemoveArticleCategories(article);
            AddArticleCategories(article);

            RemoveArticleAuthors(article);
            AddArticleAuthors(article);
            return true;
        }


        public bool DeleteArticle(int id)
        {
            var query = "DELETE FROM ArticleCategories WHERE ArticleID = " + id.ToString() + ";"
                       + "DELETE FROM ArticleAuthors WHERE ArticleID = " + id.ToString() + ";"
                        + "DELETE FROM Favorite WHERE ArticleID = " + id.ToString() + ";"
                        + "DELETE FROM Article WHERE ArticleID = " + id.ToString() + ";";

            ExecuteCommand(query);
            return true;
        }

        public Author GetAuthorById(int id)
        {
            var author = AutoSqlGenerator.Instance.Get<Author>(id);
            var sql = String.Format("SELECT a.* FROM article a, ArticleAuthors au WHERE a.articleid = au.articleid AND au.authorid ={0} ;",
                      id);
            author.PublishedArticles =
                ExecuteCommandReturnList(sql)
                    .Select(data => AutoSqlGenerator.Instance.ParseDictionary<Article>(data)).ToList();
            return author;
        }

        public IEnumerable<Author> GetAllAuthors()
        {
            var start = (new Random()).Next(500000);
            var sql = String.Format("SELECT * FROM author LIMIT 100 OFFSET {0};", start);
            var authorsData = ExecuteCommandReturnList(sql);
            return authorsData.Select(data => AutoSqlGenerator.Instance.ParseDictionary<Author>(data));
        }

        public bool AddAppUser(AppUser user)
        {
            user.Id = AutoSqlGenerator.Instance.Add(user);
            return false;
        }

        public List<AppUser> GetAppUsers()
        {
            return AutoSqlGenerator.Instance.FindAll<AppUser>(null);
        }

        public AppUser GetAppUser(string id)
        {
            return AutoSqlGenerator.Instance.Get<AppUser>(id);
        }

        public bool UpdateAppUser(AppUser appUser)
        {
            AutoSqlGenerator.Instance.Update(appUser,appUser.Id.PutIntoDollar());
            return true;
        }

        public void AddFavorite(Favorite favorite)
        {
            favorite.AdditionDate = DateTime.Now;
            AutoSqlGenerator.Instance.Add(favorite);
        }

        public Favorite GetFavorite(int articleId, string userId)
        {
            var sql = String.Format("SELECT * FROM favorite WHERE articleid={0} AND userid={1} LIMIT 1;", articleId, userId);
            var favoriteData = ExecuteCommand(sql);
            return AutoSqlGenerator.Instance.ParseDictionary<Favorite>(favoriteData);
        }

        public void RemoveFavorite(int articleId, string userId)
        {
            var sql = String.Format("DELETE FROM favorite WHERE articleid={0} AND userid={1};", articleId, userId);
            ExecuteCommand(sql);
        }

        public List<Favorite> GetFavorites(string userId)
        {
            var query = new Dictionary<string, string> { { "userid", userId } };
            var favorites = AutoSqlGenerator.Instance.FindAll<Favorite>(query);
            favorites.ForEach(favorite => favorite.Article = GetArticleById(favorite.ArticleId));
            return favorites;
        }

        public AppUser GetAppUserByUserName(string userName)
        {
            var query = new Dictionary<string, string> {{"email", userName}};
            var list = AutoSqlGenerator.Instance.FindAll<AppUser>(query);
            if(list == null || list.Count == 0)
                return null;
            return list[0];
        }

        public void VisitArticle(int articleId)
        {
            var sql = String.Format("UPDATE article SET views=views + 1 WHERE articleid={0};", articleId);
            ExecuteCommand(sql);
        }

        public void CreateVisit(Visit visit)
        {
            visit.VisitDate = DateTime.Now;
            AutoSqlGenerator.Instance.Add(visit);
        }

        public List<Visit> GetVisits(string userId)
        {
            var query = new Dictionary<string, string> {{"userid", userId}};
            var visits = AutoSqlGenerator.Instance.FindAll<Visit>(query);
            visits.ForEach(data => data.Article = GetArticleById(data.ArticleId));
            return visits;
        }

        public List<ActionHistory> GetActionsForUser(string userId)
        {
            var query = new Dictionary<string, string> { { "userid", userId } };
            var actions = AutoSqlGenerator.Instance.FindAll<ActionHistory>(query);
            actions.ForEach(action => action.Article = GetArticleById(action.ArticleId));
            return actions;
        }

        public List<ActionHistory> GetActionsForArticle(int articleId)
        {
            var query = new Dictionary<string, string> {{"articleid", articleId.ToString()}};
            var actions = AutoSqlGenerator.Instance.FindAll<ActionHistory>(query);
            return actions;
        }

        public void AddAction(ActionHistory action)
        {
            action.ActionDate = DateTime.Now;
            AutoSqlGenerator.Instance.Add(action);
        }
      
        public List<Article> GetArticles(int page, string articleName, string keyword, string authorName,
            int publicationYear, string category, string journalReference, int sortType, bool orderByDescending)
        {
            page = page < 1 ? 1 : page;
            bool authors = false;
            bool categories = false;

            int offset = (page - 1) * Global.ArticlePerPage;
            string sort = "ORDER BY " + (sortType == 1 ? "a.published " : "a.title ");
            sort = orderByDescending ? sort + " DESC " : sort;

            var conditions = new List<string>();

            #region Generating conditions
                        if (!string.IsNullOrEmpty(articleName))
                            conditions.Add(String.Format(" a.title ILIKE {0} ",MakeStringFilter(articleName)));
                        if (!string.IsNullOrEmpty(keyword))
                            conditions.Add(String.Format(" a.summary ILIKE {0} ",MakeStringFilter(keyword)));
                        if(!string.IsNullOrEmpty(journalReference))
                            conditions.Add(String.Format(" a.journalreference ILIKE {0}", MakeStringFilter(journalReference)));
           
                        if (!string.IsNullOrEmpty(authorName))
                        {
                            conditions.Add(String.Format(" articleauthors.articleid = a.articleId AND author.authorid = articleauthors.authorid AND author.authorname ILIKE {0} ", MakeStringFilter(authorName)));
                            authors = true;
                        }
                        if (!string.IsNullOrEmpty(category))
                        {
                            conditions.Add(String.Format(
                                " articlecategories.articleid = a.articleid AND articlecategories.categoryid = c.categoryId AND c.categoryName = {0} ", category.PutIntoDollar()));
                            categories = true;
                        }
                       if(publicationYear != 0)
                           conditions.Add(" date_part('year', a.published) = " + publicationYear);
            #endregion

            var conditionString = conditions.Count > 0 ? "WHERE " + String.Join(" AND ", conditions) : "";
            var sql = String.Format("SELECT a.* " +
                                    "FROM article a " + (authors
                                        ? ", articleauthors, author "
                                        : "") + (categories ? " ,articlecategories, category c" : "") +
                                    " {0} {1} LIMIT {2} OFFSET {3};", conditionString, sort, Global.ArticlePerPage, offset);

            var articlesData = ExecuteCommandReturnList(sql);
            var articles = articlesData.Select(data => AutoSqlGenerator.Instance.ParseDictionary<Article>(data)).ToList();
            articles = GetAuthorsAndCategories(articles);
           
            return articles;
        }

        public List<Article> GetArticles(ArticlesIndexViewModel model)
        {
            var articles = new List<Article>();
            var sql = "";
            switch (model.SearchType)
            {
                case "authorName":
                    sql = GetArticlesByAuthorNameSql(model);
                    break;
                case "articleCategory":
                    sql = GetArticlesByCategory(model);
                    break;
                default:
                    sql = GetArticlesByArticleData(model);
                    break;
            }
            sql = String.Format(sql,
                (model.SortType == SortTypeEnum.ByTitle ? "a.title" : "a.published"), 
                model.OrderByDescending ? "DESC" : "",
                ((model.Page - 1) * Global.ArticlePerPage),
                Global.ArticlePerPage);
            var articleData = ExecuteCommandReturnList(sql);
            articles =articleData.Select(data => AutoSqlGenerator.Instance.ParseDictionary<Article>(data)).ToList();
            articles = GetAuthorsAndCategories(articles);
            return articles;
        }

        private string GetArticlesByArticleData(ArticlesIndexViewModel model)
        {
            string key = MakeStringFilter(model.SearchKey);
            int year = 0;
            if (!string.IsNullOrEmpty(model.SearchKey))
            {
                var yearData = model.SearchKey.Where(x => x >= '0' && x <= '9').Select(y => Convert.ToInt32(y));
                yearData.ForEach(x => { year += x; year *= 10; });
            }
            string sql = "  SELECT a.* " +
                         "  FROM article a " + 
            (string.IsNullOrEmpty(model.SearchKey) ? "" : 
                         "  WHERE title ILIKE " + key +
                         "  OR summary ILIKE " + key + 
                         "  OR journalReference ILIKE " + key + 
            (year == 0 ? "" : " OR date_part('year', published) = " + year)) +
                        "ORDER BY {0} {1} LIMIT {3} OFFSET {2}; ";
            return sql;
        }

        private string GetArticlesByCategory(ArticlesIndexViewModel model)
        {
            var categoryId = ExecuteCommandScalar<int>(
                String.Format(" SELECT categoryId FROM category WHERE categoryName = {0};", 
                model.SearchKey.PutIntoDollar()));

            string sql = "  SELECT a.* " +
                         "  FROM  " +
                         " (SELECT a.* " +
                         "  FROM ArticleCategories ac, Article a " +
                         "  WHERE ac.categoryId =  " + categoryId +
                         " AND ac.articleId = a.ArticleId " +
                         " LIMIT {3} OFFSET {2} " + 
                         ") as a " +
                        "  ORDER BY {0} {1} ; ";
            return sql;
        }

        private string GetArticlesByAuthorNameSql(ArticlesIndexViewModel model)
        {
            var sql = "SELECT a.* " +
                        " FROM " +
                        " (SELECT a.* " +
                        " FROM   Article a, ArticleAuthors aa, Author aut " +
                        " WHERE   aut.AuthorName ILIKE  " + MakeStringFilter(model.SearchKey) + 
                        " AND aut.AuthorID = aa.AuthorID " +
                        " AND a.ArticleID = aa.ArticleID " +
                        " LIMIT {3} OFFSET {2} ) as a " +
                        " ORDER BY {0} {1} " +
                        " ; ";


            return sql;
        }

        public List<Article> GetAuthorsAndCategories(List<Article> articles)
        {
            if (articles == null || articles.Count < 1)
                return articles;
            List<Dictionary<string, string>> categories;
            List<Dictionary<string, string>> authors;
            var articleIds = String.Join(" OR ", articles.Select(a => "ac.articleId = " + a.ArticleId));
            var sql = "SELECT ac.articleId, c.categoryName " +
                        "FROM category c, articleCategories ac " +
                        "WHERE (" + articleIds + ") and ac.categoryId = c.categoryId;\n" +

                        "SELECT a.authorId, a.authorName, ac.articleId " +
                        "FROM author a, articleAuthors ac " +
                        "WHERE (" + articleIds + ") and ac.authorid = a.authorid;";
            using (var connection = CreateConnection())
            {
                var query = new NpgsqlCommand(sql, connection);


                Log(query.CommandText);
                var reader = query.ExecuteReader();

                categories = ReadResultSet(reader);
                reader.NextResult();
                authors = ReadResultSet(reader);
            }

            var authorsMap = new Dictionary<int, List<Dictionary<string, string>>>();
            foreach(var a in authors)
            {
                var articleId = Convert.ToInt32(a["articleid"]);
                if(!authorsMap.ContainsKey(articleId))
                    authorsMap[articleId] = new List<Dictionary<string, string>>();
                authorsMap[articleId].Add(a);
            }

            foreach (var article in articles)
            {
                article.Authors = new List<Author>();
                if(authorsMap.ContainsKey(article.ArticleId))
                    foreach (var author in authorsMap[article.ArticleId])
                    {
                        var authorId = Convert.ToInt32(author["authorid"]);
                        article.Authors.Add(new Author(authorId, author["authorname"]));
                    }
                    
            }
            var categoriesMap = new Dictionary<int, List<Dictionary<string, string>>>();
            foreach(var c in categories)
            {
                var articleId = Convert.ToInt32(c["articleid"]);
                if(!categoriesMap.ContainsKey(articleId))
                    categoriesMap[articleId] = new List<Dictionary<string, string>>();
                categoriesMap[articleId].Add(c);
            }

            foreach(var article in articles)
            {
                article.Categories = new List<string>();
                if(categoriesMap.ContainsKey(article.ArticleId))
                    foreach(var category in categoriesMap[article.ArticleId])
                        article.Categories.Add(category["categoryname"]);
            }
            return articles;
        }

        public List<Author> GetAuthorsWithName(string search)
        {
            var sql = String.Format(DatabaseConstants.SelectFromTableWhereTemplate, "author",
                " authorName ILIKE " + (search+ "%").PutIntoDollar() + "ORDER BY authorName LIMIT 15");
            var resultData = ExecuteCommandReturnList(sql);
            var result = resultData.Select(data => AutoSqlGenerator.Instance.ParseDictionary<Author>(data)).ToList();
            return result;
        }

        #region Private Helper Methods
        private string MakeStringFilter(string value)
        {
            var filter = "";
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length == 1)
                    value += ' ';
                if (value.Length == 2)
                    value = ' ' + value;
                filter = ("%" + value + "%").PutIntoDollar();
            }
            return filter;
        }

        private DateTime ParseDateTime(string postgresFormatDate)
        {
            if (String.IsNullOrEmpty(postgresFormatDate))
                return DateTime.MinValue;

            return DateTime.ParseExact(postgresFormatDate, "dd.MM.yyyy H:mm:ss",
                System.Globalization.CultureInfo.CurrentCulture);
        }
        private List<Author> GetAuthorsByArticleID(int id)
        {
            var commandGetAuthors = "SELECT a.* " +
                                    "FROM Author a, ArticleAuthors au " +
                                    "WHERE au.ArticleID = " + id.ToString() +
                                    " and a.AuthorID = au.AuthorID;";
            var articleAuthors = ExecuteCommandReturnList(commandGetAuthors);
            var authors = new List<Author>();
            foreach (var row in articleAuthors)
            {
                var author = new Author(Convert.ToInt32(row["authorid"]), row["authorname"]);
                authors.Add(author);
            }

            return authors;
        }
        private List<string> GetCategoriesByArticleID(int id)
        {
            var commandGetCategories = "SELECT c.* " +
                                       "FROM Category c, ArticleCategories ac " +
                                       "WHERE ac.ArticleID = " + id.ToString() +
                                       " and c.CategoryID = ac.CategoryID;";
            var articleCategories = ExecuteCommandReturnList(commandGetCategories);
            var categories = new List<string>();
            foreach (var row in articleCategories)
            {
                var category = row["categoryname"];
                categories.Add(category);
            }

            return categories;
        }

        private void RemoveArticleAuthors(Article article)
        {
            if (article == null)
                throw new ArgumentNullException("article");

            var query = "DELETE FROM ArticleAuthors WHERE ArticleID = "
                        + article.ArticleId + ";";

            ExecuteCommand(query);
        }
        private void AddArticleAuthors(Article article)
        {
            if (article == null)
                throw new ArgumentNullException("article");
            if (article.Authors.Count == 0)
                return;
            article.Authors
                .Where(a => a.AuthorId == 0 && !String.IsNullOrEmpty(a.AuthorName))
                .ForEach(a => a.AuthorId = Convert.ToInt32(AutoSqlGenerator.Instance.Add(a)));

            var query = DatabaseConstants.InsertTableTemplate;
            var authorsValues = String.Join(",",
                article.Authors
                .Select(a => "(" + article.ArticleId + "," + a.AuthorId + ")"));


            query = String.Format(query, "ArticleAuthors", "ArticleID, AuthorID", authorsValues, "1");
            ExecuteCommand(query);
        }
        private void AddArticleCategories(Article article)
        {
            if (article == null)
                throw new ArgumentNullException("article");
            if (article.Categories.Count == 0)
                return;

            var query = DatabaseConstants.InsertTableTemplate;
            var categoriesValue = "";
            var categories = Global.Instance.Categories.Keys.ToList();
            foreach (var category in article.Categories)
            {
                categoriesValue += "(" + article.ArticleId.ToString() + ", "
                                 + (categories.IndexOf(category) + 1) + "),";
            }
            categoriesValue = categoriesValue.Remove(categoriesValue.Length - 1);

            query = String.Format(query, "ArticleCategories", "ArticleID, CategoryID", categoriesValue, "1");
            ExecuteCommand(query);

        }
        private void RemoveArticleCategories(Article article)
        {
            if (article == null)
                throw new ArgumentNullException("article");

            var query = "DELETE FROM ArticleCategories WHERE ArticleID = "
                        + article.ArticleId.ToString() + ";";

            ExecuteCommand(query);

        }
        #endregion
    }
}