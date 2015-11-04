using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using Project_DMD.Models;
using WebGrease.Css.Extensions;

namespace Project_DMD.Classes
{
    public sealed class QueryExecutor
    {
        static readonly QueryExecutor _instance = new QueryExecutor();

   
        public static QueryExecutor Instance
        {
	        get { return _instance; }
        }

        private QueryExecutor()
        {

        }

        private DateTime parseDateTime(string postgresFormatDate)
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
            var articleAuthors = AutoSqlGenerator.Instance.ExecuteCommandReturnList(commandGetAuthors);
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
            var articleCategories = AutoSqlGenerator.Instance.ExecuteCommandReturnList(commandGetCategories);
            var categories = new List<string>();
            foreach (var row in articleCategories)
            {
                var category = row["categoryname"];
                categories.Add(category);
            }

            return categories;
        }

        private void AddArticleAuthors(Article article)
        {
            if (article == null)
                throw new ArgumentNullException("Given article cannot be null.");
            if (article.AuthorsList.Count == 0)
                return;

            var query = AutoSqlGenerator.Constants.InsertTableTemplate;
            var authorsValues = "";
            foreach (Author author in article.AuthorsList)
            {
                authorsValues += "(" + article.ArticleId.ToString() + ", "
                                 + author.AuthorId.ToString() + "),";
            }
            authorsValues = authorsValues.Remove(authorsValues.Length - 1);

            query = String.Format(query, "ArticleAuthors", "ArticleID, AuthorID", authorsValues, "1");
            AutoSqlGenerator.Instance.ExecuteCommand(query);
        }
        private void AddArticleCategories(Article article)
        {
            if (article == null)
                throw new ArgumentNullException("Given article cannot be null.");
            if (article.Categories.Count == 0)
                return;

            var query = AutoSqlGenerator.Constants.InsertTableTemplate;
            var categoriesValue = "";
            var categories = Global.Instance.Categories.Keys.ToList();
            foreach (var category in article.Categories)
            {
                categoriesValue += "(" + article.ArticleId.ToString() + ", "
                                 + (categories.IndexOf(category)+1) + "),";
            }
            categoriesValue = categoriesValue.Remove(categoriesValue.Length - 1);

            query = String.Format(query, "ArticleCategories", "ArticleID, CategoryID", categoriesValue, "1");
            AutoSqlGenerator.Instance.ExecuteCommand(query);

        }
        private void RemoveArticleCategories(Article article)
        {
            if (article == null)
                throw new ArgumentNullException("Given article cannot be null.");

            var query = "DELETE FROM ArticleCategories WHERE ArticleID = "
                        + article.ArticleId.ToString() + ";";

            AutoSqlGenerator.Instance.ExecuteCommand(query);

        }

        /// <summary>
        /// Find article by id
        /// </summary>
        /// <param name="id">id of searching article</param>
        /// <returns>Article if it exists else null</returns>
        public Article GetArticleById(int id)
        {
            var commandGetArticle = "SELECT * FROM Article WHERE Article.ArticleID = " + id.ToString() + " LIMIT 1;";
            var articleData = AutoSqlGenerator.Instance.ExecuteCommand(commandGetArticle);

            if(articleData.Count == 0)
                return null;

            var authors = GetAuthorsByArticleID(id);
            var categories = GetCategoriesByArticleID(id);

            return new Article()
                .WithId(Convert.ToInt32(articleData["articleid"]))
                .WithTitle(articleData["title"])
                .WithSummary(articleData["summary"])
                .WithPublished(parseDateTime(articleData["published"]))
                .WithUpdate(parseDateTime(articleData["updated"]))
                .WithViews(Convert.ToInt32(articleData["views"]))
                .WithDoi(articleData["doi"])
                .WithJournalReference(articleData["journalreference"])
                .WithAuthors(authors)
                .WithCategories(categories)
                .WithUrl(articleData["url"]);
        }

        /// <summary>
        /// Adding article to DB
        /// </summary>
        /// <param name="article">Article to add</param>
        /// <returns>Id of created article</returns>
        public int AddArticle(Article article)
        {
            article.WithPublished(DateTime.Now);

            string query = AutoSqlGenerator.Constants.InsertTableTemplateWithoutColumns;
            query = String.Format(query, "Article",  article.ToSql(), "ArticleID");

            var queryData = AutoSqlGenerator.Instance.ExecuteCommand(query);
            
            if (queryData == null)
                throw new InvalidDataException("Given article is not valid. (ID isn't presented in table, invalid date and so on)");

            article.WithId(Convert.ToInt32(queryData["articleid"]));

            AddArticleAuthors(article);
            AddArticleCategories(article);

            return article.ArticleId;
        }

        /// <summary>
        ///  Updating given article in DB
        /// </summary>
        /// <param name="article"></param>
        /// <returns>True if article successfully updated, else false</returns>
        public bool UpdateArticle(Article article)
        {
            Article oldArticle = GetArticleById(article.ArticleId);

            article.WithUpdate(DateTime.Now)
                .WithPublished(oldArticle.Published);

            var query = AutoSqlGenerator.Constants.UpdateOnTemplate;
            var values = "(ArticleID, Title, Summary, Published, Updated, Views, URL, DOI, JournalReference) = " +
                         article.ToSql();

            query = String.Format(query, "Article", values, "ArticleID = " + article.ArticleId.ToString());
            AutoSqlGenerator.Instance.ExecuteCommand(query);

            RemoveArticleCategories(article);
            AddArticleCategories(article);

            return true;
        }

        /// <summary>
        /// Deletes article from table
        /// </summary>
        /// <param name="id">Id of article</param>
        /// <returns>true if article successfully deleted, else false</returns>
        public bool DeleteArticle(int id)
        {
            var query = "DELETE FROM ArticleCategories WHERE ArticleID = " + id.ToString() + ";"
                       + "DELETE FROM ArticleAuthors WHERE ArticleID = " + id.ToString() + ";"
                        + "DELETE FROM Favorite WHERE ArticleID = " + id.ToString() + ";"
                        + "DELETE FROM Article WHERE ArticleID = " + id.ToString() + ";";

            AutoSqlGenerator.Instance.ExecuteCommand(query);
            return true;
        }

        /// <summary>
        /// Get author by his Id
        /// </summary>
        /// <param name="id">Id of needed author</param>
        /// <returns>Null if author doesn't exist in table, else author data</returns>
        public Author GetAuthorById(int id)
        {
            var author = AutoSqlGenerator.Instance.Get<Author>(id);
            var sql = String.Format("SELECT a.* FROM article a, ArticleAuthors au WHERE a.articleid = au.articleid AND au.authorid ={0} ;",
                      id);
            author.PublishedArticles =
                AutoSqlGenerator.Instance.ExecuteCommandReturnList(sql)
                    .Select(data => AutoSqlGenerator.Instance.ParseDictionary<Article>(data)).ToList();
            return author;
        }

        /// <summary>
        /// Get list of authors
        /// </summary>
        /// <returns>List of authors</returns>
        public IEnumerable<Author> GetAllAuthors()
        {
            var start = (new Random()).Next(500000);
            var sql = String.Format("SELECT * FROM author LIMIT 100 OFFSET {0};", start);
            var authorsData = AutoSqlGenerator.Instance.ExecuteCommandReturnList(sql);
            return authorsData.Select(data => AutoSqlGenerator.Instance.ParseDictionary<Author>(data));
        }

        /// <summary>
        /// Adding new AppUser to table
        /// </summary>
        /// <param name="user">Given AppUser</param>
        /// <returns>True if appUser successfully added into table, otherwise false </returns>
        public bool AddAppUser(AppUser user)
        {
            user.Id = AutoSqlGenerator.Instance.Add(user);
            return false;
        }

        /// <summary>
        /// Getting AppUsers list
        /// </summary>
        /// <returns>List of AppUsers</returns>
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
            AutoSqlGenerator.Instance.Update(appUser,appUser.Id.PutIntoQuotes());
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
            var favoriteData = AutoSqlGenerator.Instance.ExecuteCommand(sql);
            return AutoSqlGenerator.Instance.ParseDictionary<Favorite>(favoriteData);
        }

        public void RemoveFavorite(int articleId, string userId)
        {
            var sql = String.Format("DELETE FROM favorite WHERE articleid={0} AND userid={1};", articleId, userId);
            AutoSqlGenerator.Instance.ExecuteCommand(sql);
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
            var query = new Dictionary<string, string> {{"email", userName.PutIntoQuotes()}};
            var list = AutoSqlGenerator.Instance.FindAll<AppUser>(query);
            if(list == null || list.Count == 0)
                return null;
            return list[0];
        }

        public void VisitArticle(int articleId)
        {
            var sql = String.Format("UPDATE article SET views=views + 1 WHERE articleid={0};", articleId);
            AutoSqlGenerator.Instance.ExecuteCommand(sql);
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="articleName"></param>
        /// <param name="keyword"></param>
        /// <param name="authorName"></param>
        /// <param name="publicationYear"></param>
        /// <param name="category"></param>
        /// <param name="journalReference"></param>
        /// <param name="sortType">1 - sort by date; 0 - sort by title</param>
        /// <param name="orderByDescending"></param>
        /// <returns></returns>
        public List<Article> GetArticles(int page, string articleName, string keyword, string authorName,
            int publicationYear, string category, string journalReference, int sortType, bool orderByDescending)
        {
            bool authors = false;
            bool categories = false;

            int offset = (page - 1) * Global.ArticlePerPage;
            string sort = "ORDER BY " + (sortType == 1 ? "a.published " : "a.title ");
            sort = orderByDescending ? sort + " DESC " : sort;

            var conditions = new List<string>();

            #region Generating conditions
                        if (!string.IsNullOrEmpty(articleName))
                            conditions.Add(String.Format(" a.title ILIKE {0} ",makeStringFilter(articleName)));
                        if (!string.IsNullOrEmpty(keyword))
                            conditions.Add(String.Format(" a.summary ILIKE {0} ",makeStringFilter(keyword)));
                        if(!string.IsNullOrEmpty(journalReference))
                            conditions.Add(String.Format(" a.journalreference ILIKE {0}", makeStringFilter(journalReference)));
           
                        if (!string.IsNullOrEmpty(authorName))
                        {
                            conditions.Add(String.Format(" articleauthors.articleid = a.articleId AND author.authorid = articleauthors.authorid AND author.authorname ILIKE {0} ", makeStringFilter(authorName)));
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

            var articlesData = AutoSqlGenerator.Instance.ExecuteCommandReturnList(sql);
            var articles = articlesData.Select(data => AutoSqlGenerator.Instance.ParseDictionary<Article>(data)).ToList();
            foreach (var a in articles)
            {
                 a.AuthorsList = GetAuthorsByArticleID(a.ArticleId);
                a.Categories = GetCategoriesByArticleID(a.ArticleId);
            }
            return articles;
        }

        private string makeStringFilter(string value)
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
    }
}