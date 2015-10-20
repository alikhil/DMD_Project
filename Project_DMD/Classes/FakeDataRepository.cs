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
                    AuthorName = "Alik Khil"
                },
                new Author()
                {
                    AuthorId = 1,
                    AuthorName = "Nikita Shib"
                },
                new Author()
                {
                    AuthorId = 2,
                    AuthorName = "Timur Khazh"
                },
                new Author()
                {
                    AuthorId = 3,
                    AuthorName = "Ruslan Tush"
                }
            });

            ArticlesList.AddRange(new [] {
                new Article()
                {
                    ArticleId = 0,
                    DOI = "ADAD",
                    Published = DateTime.Now,
                    Summary = "Summary",
                    Title = "Geek best 3",
                    Updated = DateTime.Now,
                    Url = "http://vk.com",
                    JournalReference = "BBC",
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
                    Title = "Best of habr",
                    Updated = DateTime.Now,
                    Url = "http://vk.com",
                    JournalReference = "BBC",
                    Authors = new List<Author>(
                        new[] {
                            Authors[2], 
                            Authors[3],
                            Authors[1]
                        })
                },
                new Article()
                {
                    ArticleId = 2,
                    DOI = "EWWG",
                    Published = DateTime.Now,
                    Summary = "SuSDFmmary",
                    Title = "TOuch of class",
                    Updated = DateTime.Now,
                    Url = "http://vk.com/sdf",
                    JournalReference = "DSFSD",
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

        public int Add(Article article)
        {
            article.ArticleId = ArticlesList.Count;
            ArticlesList.Add(article);
            return article.ArticleId;
        }

        public Article GetArticle(int id)
        {
            return (Article)getArticle(id).Clone();
        }

        public List<Article> GetArticles()
        {
            return ArticlesList;
        }

        public void Update(Article article)
        {
            int index = ArticlesList.FindIndex(x => x.ArticleId == article.ArticleId);
            var old = ArticlesList[index];
            old.DOI = article.DOI;
            old.Title = article.Title;
            old.Url = article.Url;
            old.JournalReference = article.JournalReference;
        }

        public void Delete(Article article)
        {
            ArticlesList.Remove(article);
        }

        public void Delete(int id)
        {
            ArticlesList.Remove(getArticle(id));
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


        public void VisitArticle(int articleId)
        {
            var article = getArticle(articleId);
            article.Views++;
        }

        private Article getArticle(int articleId)
        {
            return ArticlesList.Find(x => x.ArticleId == articleId);
        }
    }

}