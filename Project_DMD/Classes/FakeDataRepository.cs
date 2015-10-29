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
        private List<Article> _articlesList = new List<Article>();
        private readonly List<Author> _authors = new List<Author>();

        public FakeDataRepository()
        {
            #region Generating fake data
            _authors = new List<Author>(new[] {
                new Author(0, "Alik Khil"),
                new Author(1, "Nikita Shib"),
                new Author(2, "Timur Khazh"),
                new Author(3, "Ruslan Tush")
            });

            _articlesList.AddRange(new [] {
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
                        _authors[0],_authors[1]
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
                            _authors[2], _authors[3], _authors[1]
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
                            _authors[0], _authors[2], _authors[1]
                        })
            });

            foreach (var art in _articlesList)
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
            article.ArticleId = _articlesList.Count;
            article.AuthorsList = GetFullAuthorInfo(article);
            _articlesList.Add(article);
            return article.ArticleId;
        }

        private List<Author> GetFullAuthorInfo(Article article)
        {
            return article.AuthorsList.Select(author => GetAuthor(author.AuthorId)).ToList();
        }

        public Article GetArticle(int id)
        {
            return (Article)getArticle(id).Clone();
        }

        public List<Article> GetArticles()
        {
            return _articlesList;
        }

        public void Update(Article article)
        {
            int articleIndex = _articlesList.FindIndex(x => x.ArticleId == article.ArticleId);
            var oldArticleVersion = _articlesList[articleIndex];
            oldArticleVersion.WithDoi(article.DOI)
                .WithUrl(article.Url)
                .WithJournalReference(article.JournalReference)
                .WithCategories(article.Categories)
                .WithTitle(article.Title);
        }

        public void Delete(Article article)
        {
            _articlesList.Remove(article);
        }

        public void Delete(int id)
        {
            _articlesList.Remove(getArticle(id));
        }

        public Author GetAuthor(int id)
        {
            id = (id + _authors.Count) % _authors.Count;
            return _authors[id];
        }

        public List<Author> GetAuthors()
        {
            return _authors;
        }

        public void Dispose()
        {
            _articlesList = null;
        }


        public void VisitArticle(int articleId)
        {
            var article = getArticle(articleId);
            article.Views++;
        }

        
      public List<Article> GetArticles(int page, string articleName, string keyword, string authorName, int publicationYear, string category,
            string journalReference, int sortType, bool orderByDescending)
      {
            var result = _articlesList.FilterByArticleName(articleName)
                .FilterByAuthors(authorName)
                .FilterByCategory(category)
                .FilterByKeyword(keyword)
                .FilterByPublicationYear(publicationYear)
                .FilterByJournalReference(journalReference);
           
            var takeFrom = (page - 1) * Global.ArticlePerPage;

          // by publish date
          if (sortType == 1)
              result = orderByDescending
                  ? result.OrderByDescending(article => article.Published).ToList()
                  : result.OrderBy(article => article.Published).ToList();
          else
              //by article title
              result = orderByDescending
                  ? result.OrderByDescending(article => article.Title).ToList()
                  : result.OrderBy(article => article.Title).ToList();
                  
            return result.Skip(takeFrom).Take(Global.ArticlePerPage).ToList();
        }

        

        private Article getArticle(int articleId)
        {
            return _articlesList.Find(x => x.ArticleId == articleId);
        }
    }

    /// <summary>
    /// Extension class for list of articles, for more efficeint filtering articles
    /// </summary>
    public static class Filter
    {
        public static List<Article> FilterByJournalReference(this List<Article> result, string journalReference)
        {
            if (!string.IsNullOrEmpty(journalReference))
                result = result.FindAll(x => x.JournalReference.Contains(journalReference));
            return result;
        }

        public static List<Article> FilterByAuthors(this List<Article> result, string authorName)
        {
            if (!string.IsNullOrEmpty(authorName))
                result = result.FindAll(x => x.AuthorsList.Exists(y => authorName.Contains(y.AuthorName)));
            return result;
        }

        public static List<Article> FilterByCategory(this List<Article> result, string category)
        {
            if (!string.IsNullOrEmpty(category) && Global.Instance.Categories.ContainsKey(category))
                result = result.FindAll(x => x.Categories.Exists(y => y == category));
            return result;
        }

        public static List<Article> FilterByPublicationYear(this List<Article> result, int publicationYear)
        {
            if (publicationYear != 0)
                result = result.FindAll(x => x.Published.Year == publicationYear);
            return result;
        }

        public static List<Article> FilterByKeyword(this List<Article> result,string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                result = result.FindAll(x => x.Summary.Contains(keyword));
            return result;
        }

        public static List<Article> FilterByArticleName(this List<Article> result, string articleName)
        {
            if (!string.IsNullOrEmpty(articleName))
                result = result.FindAll(x => x.Title == articleName);
            return result;
        }
    }
}