using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_DMD.Models
{
    public class ArticlesIndexViewModel
    {
        public string ArticleTitle { get; set; }

        public int PublicationYear { get; set; }

        public string ArticleSummary { get; set; }

        public string JournalReference { get; set; }

        public string AuthorName{ get; set; }

        public string CategoryName { get; set; }

        public int Page { get; set; }

        public bool OrderByDescending { get; set; }

        public SortTypeEnum SortType{ get; set; }

        public List<Article> Articles { get; set; }

        public double ElapsedTime { get; set; }
    }

    public enum SortTypeEnum
    {
        ByTitle,
        ByPublicationYear
    }
}