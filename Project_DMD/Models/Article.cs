﻿using System;
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
        public string Venue { get; set; }

        /// <summary>
        /// DOI
        /// </summary>
        public string DOI { get; set; }
    }

}