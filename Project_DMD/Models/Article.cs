using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [MaxLength(65356)]
        public string Url { get; set; }

        /// <summary>
        /// Title of an article
        /// </summary>
        [Required]
        [StringLength(512, MinimumLength = 5)]
        public string Title { get; set; }

        /// <summary>
        /// Short description
        /// </summary>
        [Required]
        [MaxLength(2048)]
        public string Summary { get; set; }

        /// <summary>
        /// Date of publishing an article
        /// </summary>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime Published { get; set; }

        /// <summary>
        /// Date of updating article
        /// </summary>
        public DateTime Updated { get; set; }

        /// <summary>
        /// Conference or journal name
        /// </summary>
        [MaxLength(512)]
        public string JournalReference { get; set; }

        /// <summary>
        /// DOI
        /// </summary>
        [MaxLength(512)]
        public string DOI { get; set; }

        /// <summary>
        /// Number of article view 
        /// </summary>
        public int Views { get; set; }

        /// <summary>
        /// Authors of article
        /// </summary>
        [Required]
        public List<Author> Authors { get; set; }

        /// <summary>
        /// Colection of articles catrgories 
        /// </summary>
        [Required]
        public List<string> Categories { get; set; }

    }

}