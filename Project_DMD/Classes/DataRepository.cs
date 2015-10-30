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

        void Update(Article article);

        void Delete(Article article);

        int Add(Article article);

        void Delete(int id);

        Author GetAuthor(int id);

        List<Author> GetAuthors();

        void Dispose();

        void VisitArticle(int articleId);

        List<Article> GetArticles(int page, string articleName, string keyword, string authorName, int publicationYear, string category, string journalReference, int sortType, bool orderByDescending);
    }

    /// <summary>
    /// For repository for articles and authors
    /// </summary>
    public class DataRepository : IDataRepository
    {

        public int Add(Article article)
        {
            return QueryExecutor.Instance.AddArticle(article);
        }

        public Article GetArticle(int id)
        {
            return QueryExecutor.Instance.GetArticleById(id);
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

        public Author GetAuthor(int id)
        {
            return QueryExecutor.Instance.GetAuthorById(id);
        }

        public List<Author> GetAuthors()
        {
            return QueryExecutor.Instance.GetAuthors();
        }

        public void Dispose()
        {
        }


        public void VisitArticle(int articleId)
        {
            QueryExecutor.Instance.VisitArticle(articleId);
        }

        public List<Article> GetArticles(int page, string articleName, string keyword, string authorName, int publicationYear, string category,
            string journalReference, int sortType, bool orderByDescending)
        {
            return QueryExecutor.Instance.GetArticles(page, articleName, keyword, authorName, publicationYear, category, sortType, orderByDescending);

        }

    }
}