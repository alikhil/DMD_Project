using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
                .WithId(0)
                .WithDoi("asda")
                .WithTitle("Geek best 3")
                .WithSummary("Summary")
                .WithUrl("vk.com/alikgil")
                .WithUpdate(DateTime.Now)
                .WithPublished(DateTime.Now)
                .WithCategories(new List<string>{"stat.AP"})
                .WithJournalReference("HAKATONE")
                .WithAuthors(
                    new List<Author>
                    {
                        Authors[0],Authors[1]
                    }),

                new Article()
                    .WithId(1)
                    .WithTitle("Best of habr")
                    .WithSummary("Sum dary")
                    .WithUrl("vk.com/habr")
                    .WithJournalReference("Zum la")
                    .WithPublished(DateTime.Now)
                    .WithUpdate(DateTime.Now)
                    .WithDoi("asdij")
                    .WithCategories(new List<string>{"stat.CO","stat.AP"})
                    .WithAuthors(
                        new List<Author>
                        {
                            Authors[2], Authors[3], Authors[1]
                        }),

                new Article()
                    .WithId(2)
                    .WithTitle("Touch of class")
                    .WithSummary("Super book")
                    .WithDoi("Azaaz")
                    .WithUpdate(DateTime.Now)
                    .WithPublished(DateTime.Now)
                    .WithUrl("habr.com")
                    .WithJournalReference("Eifil studi")
                    .WithCategories(
                        new List<string>{ "stat.CO" })
                        .WithAuthors(new List<Author>
                        {
                            Authors[0], Authors[2], Authors[1]
                        })
            });

            foreach (var art in ArticlesList)
            {
                foreach (var auth in art.AuthorsList)
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
            article.AuthorsList = article.Authors.Select(GetAuthor).ToList();
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
            old.WithDoi(article.DOI)
                .WithUrl(article.Url)
                .WithJournalReference(article.JournalReference)
                .WithCategories(article.Categories)
                .WithTitle(article.Title)
                .WithAuthors(article.AuthorsList);
            old.DOI = article.DOI;
            old.Title = article.Title;
            old.Url = article.Url;
            old.JournalReference = article.JournalReference;
            old.Categories = article.Categories;
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
            id = (id + Authors.Count) % Authors.Count;
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

        public List<Article> GetArticles(string articleName, string keyword, string authorName, int publicationYear, string category)
        {
            var result = ArticlesList;
            if(!string.IsNullOrEmpty(articleName))
                result = result.FindAll(x => x.Title == articleName);
            if (!string.IsNullOrEmpty(keyword))
                result = result.FindAll(x => x.Summary.Contains(keyword));
            if (publicationYear != 0)
                result = result.FindAll(x => x.Published.Year == publicationYear);
            if (!string.IsNullOrEmpty(category) && Global.Instance.Categories.ContainsKey(category))
                result = result.FindAll(x => x.Categories.Exists(y => y == category));
            if (!string.IsNullOrEmpty(authorName))
                result = result.FindAll(x => x.AuthorsList.Exists(y => y.AuthorName == authorName));
            return result;
        }

        private Article getArticle(int articleId)
        {
            return ArticlesList.Find(x => x.ArticleId == articleId);
        }
    }

}