using Project_DMD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_DMD.Classes
{
    public interface IDataRepository
    {
        Article GetArticle(int id);

        List<Article> GetArticles();

        void Update(Article article);

        void Delete(Article article);

        void Add(Article article);

        void Delete(int id);

        void Dispose();
    }

    public class DataRepository : IDataRepository
    {

        public void Add(Article article)
        {
            QueryExecutor.Instance.AddArticle(article);
        }

        public Article GetArticle(int id)
        {
            return QueryExecutor.Instance.GetArticleById(id);
        }

        public List<Article> GetArticles()
        {
            return QueryExecutor.Instance.GetArticles();
        }

        public void Update(Article article)
        {
            QueryExecutor.Instance.UpdateArticle(article);
        }

        public void Delete(Article article)
        {
            Delete(article.ArticleId);
        }

        public void Delete(int id)
        {
            QueryExecutor.Instance.DeleteArticle(id);
        }

        public void Dispose()
        {

        }
    }
}