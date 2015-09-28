using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project_DMD.Models;

namespace Project_DMD.Classes
{
    public class FakeDataRepository : IDataRepository
    {
        private List<Article> ArticlesList = new List<Article>();
        private List<Author> Authors = new List<Author>();

        public FakeDataRepository()
        {
            #region Generating fake data
            Authors = new List<Author>(new[] {
                new Author()
                {
                    AuthorId = 0,
                    Name = "Alik Khil"
                },
                new Author()
                {
                    AuthorId = 1,
                    Name = "Nikita Shib"
                },
                new Author()
                {
                    AuthorId = 2,
                    Name = "Timur Khazh"
                },
                new Author()
                {
                    AuthorId = 3,
                    Name = "Ruslan Tush"
                }
            });

            ArticlesList.AddRange(new [] {
                new Article()
                {
                    ArticleId = 0,
                    DOI = "ADAD",
                    Published = DateTime.Now,
                    Summary = "Summary",
                    Title = "Title",
                    Updated = DateTime.Now,
                    Url = "http://vk.com",
                    Venue = "BBC",
                    Authors = new List<Author>(
                        new[] {
                            Authors[0], 
                            Authors[1]
                        })
                },
                new Article()
                {
                    ArticleId = 1,
                    DOI = "ADAD",
                    Published = DateTime.Now,
                    Summary = "Summary",
                    Title = "Title",
                    Updated = DateTime.Now,
                    Url = "http://vk.com",
                    Venue = "BBC",
                    Authors = new List<Author>(
                        new[] {
                            Authors[2], 
                            Authors[3],
                            Authors[1]
                        })
                },
                new Article()
                {
                    ArticleId = 1,
                    DOI = "EWWG",
                    Published = DateTime.Now,
                    Summary = "SuSDFmmary",
                    Title = "TiSDtle",
                    Updated = DateTime.Now,
                    Url = "http://vk.com/sdf",
                    Venue = "DSFSD",
                    Authors = new List<Author>(
                        new[] {
                            Authors[0], 
                            Authors[2],
                            Authors[1]
                        })
                }
            });
            foreach (Article art in ArticlesList)
            {
                foreach (Author auth in art.Authors)
                {
                    auth.PublishedArticles = auth.PublishedArticles ?? new List<Article>();
                    auth.PublishedArticles.Add(art);
                }
            }
            #endregion
        }

        public void Add(Article article)
        {
            ArticlesList.Add(article);
        }

        public Article GetArticle(int id)
        {
            return ArticlesList.Find(x => x.ArticleId == id);
        }

        public List<Article> GetArticles()
        {
            return ArticlesList;
        }

        public void Update(Article article)
        {
            int index = ArticlesList.FindIndex(x => x.ArticleId == article.ArticleId);
            ArticlesList[index] = article;
        }

        public void Delete(Article article)
        {
            ArticlesList.Remove(article);
        }

        public void Delete(int id)
        {
            ArticlesList.Remove(GetArticle(id));
        }

        public Author GetAuthor(int id)
        {
            return Authors[id];
        }

        public List<Author> GetAuthors()
        {
            return Authors;
        }

        public void Dispose()
        {
            ArticlesList = null;
        }
    }

}