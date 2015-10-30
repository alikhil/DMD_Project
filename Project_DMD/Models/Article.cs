using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Owin.Security.OAuth.Messages;

namespace Project_DMD.Models
{
    public class Article
    {
        public int ArticleId { get; set; }

        /// <summary>
        /// Link to the article in arxiv
        /// </summary>
        [MaxLength(65356)]
        public string Url { get; set; }

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

        public List<Author> AuthorsList { get; set; }

        [Required]
        public int[] Authors { get; set; }

        [Required]
        public List<string> Categories { get; set; }

        #region fluent interface

        public Article WithId(int id)
        {
            ArticleId = id;
            return this;
        }

        public Article WithDoi(string doi)
        {
            DOI = doi;
            return this;
        }

        public Article WithSummary(string summary)
        {
            Summary = summary;
            return this;
        }

        public Article WithTitle(string title)
        {
            Title = title;
            return this;
        }

        public Article WithUrl(string url)
        {
            Url = url;
            return this;
        }

        public Article WithAuthors(List<Author> authors)
        {
            AuthorsList = authors;
            return this;
        }

        public Article WithCategories(List<string> categories)
        {
            Categories = categories;
            return this;
        }

        public Article WithJournalReference(string journalReference)
        {
            JournalReference = journalReference;
            return this;
        }

        public Article WithUpdate(DateTime updateTime)
        {
            Updated = updateTime;
            return this;
        }

        public Article WithPublished(DateTime publishedDate)
        {
            Published = publishedDate;
            return this;
        }

        public Article WithViews(int viewsNumber)
        {
            Views = viewsNumber;
            return this;
        }

        #endregion

    }

    
}