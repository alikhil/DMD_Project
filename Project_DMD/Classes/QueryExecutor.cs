using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Project_DMD.Models;

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
        /// <summary>
        /// Find article by id
        /// </summary>
        /// <param name="id">id of searching article</param>
        /// <returns>Article if it exists else null</returns>
        public Article GetArticleById(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Getting all articles in DB
        /// </summary>
        /// <returns>List of articles</returns>
        public List<Article> GetArticles()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adding article to DB
        /// </summary>
        /// <param name="article">Article to add</param>
        /// <returns>True if article successfully added, else false</returns>
        public bool AddArticle(Article article)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///  Updating given article in DB
        /// </summary>
        /// <param name="article"></param>
        /// <returns>True if article successfully updated, else false</returns>
        public bool UpdateArticle(Article article)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes article from table
        /// </summary>
        /// <param name="id">Id of article</param>
        /// <returns>true if article successfully deleted, else false</returns>
        public bool DeleteArticle(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get author by his Id
        /// </summary>
        /// <param name="id">Id of needed author</param>
        /// <returns>Null if author doesn't exist in table, else author data</returns>
        public Author GetAuthorById(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get list of authors
        /// </summary>
        /// <returns>List of authors</returns>
        public List<Author> GetAuthors()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adding new AppUser to table
        /// </summary>
        /// <param name="user">Given AppUser</param>
        /// <returns>True if appUser successfully added into table, otherwise false </returns>
        public bool AddAppUser(AppUser user)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Getting AppUsers list
        /// </summary>
        /// <returns>List of AppUsers</returns>
        public List<AppUser> GetAppUsers()
        {
            throw new NotImplementedException();
        }

        internal AppUser GetAppUser(string id)
        {
            throw new NotImplementedException();
        }

        internal bool UpdateAppUser(AppUser appUser)
        {
            throw new NotImplementedException();
        }

        internal void AddFavorite(Favorite favorite)
        {
            throw new NotImplementedException();
        }

        internal Favorite GetFavorite(int articleId, string userId)
        {
            throw new NotImplementedException();
        }

        internal void RemoveFavorite(int articleId, string userId)
        {
            throw new NotImplementedException();
        }

        internal List<Favorite> GetFavorites(string userId)
        {
            throw new NotImplementedException();
        }

        internal AppUser GetAppUserByUserName(string userName)
        {
            throw new NotImplementedException();
        }

        internal void VisitArticle(int articleId)
        {
            throw new NotImplementedException();
        }

        internal void CreateVisit(Visit visit)
        {
            throw new NotImplementedException();
        }

        internal List<Visit> GetVisits(string userId)
        {
            throw new NotImplementedException();
        }
    }
}