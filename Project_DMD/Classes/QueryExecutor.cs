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
        public Article GetArticleById(int id)
        {
            throw new NotImplementedException();
        }

        public List<Article> GetArticles()
        {
            throw new NotImplementedException();
        }

        public void AddArticle(Article article)
        {
            throw new NotImplementedException();
        }

        public void UpdateArticle(Article article)
        {
            throw new NotImplementedException();
        }

        public void DeleteArticle(int id)
        {
            throw new NotImplementedException();
        }

        public Author GetAuthorById(int id)
        {
            throw new NotImplementedException();
        }

        public List<Author> GetAuthors()
        {
            throw new NotImplementedException();
        }

        internal void AddAppUser(AppUser user)
        {
            throw new NotImplementedException();
        }
    }
}