﻿using System;
using System.Collections.Generic;

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
        /// <returns>Id of created article</returns>
        public int AddArticle(Article article)
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

        public AppUser GetAppUser(string id)
        {
            throw new NotImplementedException();
        }

        public bool UpdateAppUser(AppUser appUser)
        {
            throw new NotImplementedException();
        }

        public void AddFavorite(Favorite favorite)
        {
            throw new NotImplementedException();
        }

        public Favorite GetFavorite(int articleId, string userId)
        {
            throw new NotImplementedException();
        }

        public void RemoveFavorite(int articleId, string userId)
        {
            throw new NotImplementedException();
        }

        public List<Favorite> GetFavorites(string userId)
        {
            throw new NotImplementedException();
        }

        public AppUser GetAppUserByUserName(string userName)
        {
            throw new NotImplementedException();
        }

        public void VisitArticle(int articleId)
        {
            throw new NotImplementedException();
        }

        public void CreateVisit(Visit visit)
        {
            throw new NotImplementedException();
        }

        public List<Visit> GetVisits(string userId)
        {
            throw new NotImplementedException();
        }

        public List<ActionHistory> GetActionsForUser(string userId)
        {
            throw new NotImplementedException();
        }

        public List<ActionHistory> GetActionsForArticle(int articleId)
        {
            throw new NotImplementedException();
        }

        public void AddAction(ActionHistory action)
        {
            throw new NotImplementedException();
        }

        public List<Article> GetArticles(int page, string articleName, string keyword, string authorName,
            int publicationYear, string category, int sortType, bool orderByDescending)
        {
            throw new NotImplementedException();
        }
    }
}