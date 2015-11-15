using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project_DMD.Models;

namespace Project_DMD.Repositories
{
    public interface IDataRepository
    {
        Article GetArticle(int id);

        void Update(Article article);

        void Delete(Article article);

        int Add(Article article);

        void Delete(int id);

        Author GetAuthor(int id);

        IEnumerable<Author> GetAuthors();

        void Dispose();

        void VisitArticle(int articleId);

        List<Article> GetArticles(int page, string articleName, string keyword, string authorName, int publicationYear, string category, string journalReference, int sortType, bool orderByDescending);
        
        List<Author> GetAuthorsByName(string search);
    }

}