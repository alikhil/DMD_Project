using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project_DMD.Models;

namespace Project_DMD.Classes
{
    public class FakeDataRepository : IDataRepository
    {
        private List<Article> List = new List<Article>();
        
        public FakeDataRepository()
        {
            List.Add(new Article()
            {
                ArticleId = 0,
                DOI = "ADAD",
                Published = DateTime.Now,
                Summary = "Summary",
                Title = "Title",
                Updated = DateTime.Now,
                Url = "http://vk.com",
                Venue = "BBC"
            });
            List.Add(new Article()
            {
                ArticleId = 1,
                DOI = "ADAD",
                Published = DateTime.Now,
                Summary = "Summary",
                Title = "Title",
                Updated = DateTime.Now,
                Url = "http://vk.com",
                Venue = "BBC"
            });
        }
        public void Add(Article article)
        {
            List.Add(article);
        }

        public Article GetArticle(int id)
        {
            return List.Find(x => x.ArticleId == id);
        }

        public List<Article> GetArticles()
        {
            return List;
        }

        public void Update(Article article)
        {
            int index = List.FindIndex(x => x.ArticleId == article.ArticleId);
            List[index] = article;
        }

        public void Delete(Article article)
        {
            List.Remove(article);
        }

        public void Delete(int id)
        {
            List.Remove(GetArticle(id));
        }

        public void Dispose()
        {
            List = null;
        }
    }

}