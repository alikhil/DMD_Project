using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Project_DMD.Models
{
    public class Article
    {
        /// <summary>
        /// Id of an article
        /// </summary>
        
        public int ArticleId { get; set; }

        /// <summary>
        /// Link to the article in arxiv
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Title of an article
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Short description
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Date of publishing an article
        /// </summary>
        public DateTime Published { get; set; }

        /// <summary>
        /// Date of updating article
        /// </summary>
        public DateTime Updated { get; set; }

        /// <summary>
        /// Conference or journal name
        /// </summary>
        public string JournalReference { get; set; }

        /// <summary>
        /// DOI
        /// </summary>
        public string DOI { get; set; }

        /// <summary>
        /// Number of article view 
        /// </summary>
        public int Views { get; set; }

        /// <summary>
        /// Authors of article
        /// </summary>
        public List<Author> Authors { get; set; }

        /// <summary>
        /// Colection of articles catrgories 
        /// </summary>
        public List<Category> Categories { get; set; } 
    }

}