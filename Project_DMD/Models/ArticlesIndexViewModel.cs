using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_DMD.Models
{
    public class ArticlesIndexViewModel
    {
        public string SearchKey { get; set; }

        public string SearchType { get; set; }

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