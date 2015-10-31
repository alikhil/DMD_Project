using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Owin.Security.OAuth.Messages;
using Project_DMD.Attributes;
using Project_DMD.Classes;

namespace Project_DMD.Models
{
    [AgsModel]
    public class Article
    {
        [AgsPrimary]
        public int ArticleId { get; set; }

        /// <summary>
        /// Link to the article in arxiv
        /// </summary>
        [MaxLength(65356)]
        [AgsAttribute]
        public string Url { get; set; }

        [Required]
        [StringLength(512, MinimumLength = 5)]
        [AgsAttribute]
        public string Title { get; set; }

        /// <summary>
        /// Short description
        /// </summary>
        [Required]
        [MaxLength(2048)]
        [AgsAttribute]
        public string Summary { get; set; }

        /// <summary>
        /// Date of publishing an article
        /// </summary>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        [AgsAttribute]
        public DateTime Published { get; set; }

        /// <summary>
        /// Date of updating article
        /// </summary>
        [AgsAttribute]
        public DateTime Updated { get; set; }

        /// <summary>
        /// Conference or journal name
        /// </summary>
        [MaxLength(512)]
        [AgsAttribute]
        public string JournalReference { get; set; }

        /// <summary>
        /// DOI
        /// </summary>
        [MaxLength(512)]
        [AgsAttribute]
        public string DOI { get; set; }

        /// <summary>
        /// Number of article view 
        /// </summary>
        [AgsAttribute]
        public int Views { get; set; }

        [AgsForeign(Name = "ArticlId", TableName = "ArticleAuthors")]
        public List<Author> AuthorsList { get; set; }

        [Required]
        public int[] Authors { get; set; }

        [Required]
        [AgsForeign(Name = "ArticleId", TableName = "ArticleCategories")]
        public List<string> Categories { get; set; }

        public Article()
        {
            Views = 0;
            DOI = String.Empty;
            JournalReference = String.Empty;
            Url = String.Empty;
            Summary = String.Empty;
            Title = String.Empty;
        }

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

        /// <summary>
        /// Returns string in tuple format. (Column1Value, Column2Value, ColumnNValue)
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string articleData = "(" + ArticleId.ToString() + ", "
                                 + Title.ToString() + ", "
                                 + Summary.ToString() + ", "
                                 + Published.ToString("dd.MM.yyyy") + ", "
                                 + Updated.ToString("dd.MM.yyyy") + ", "
                                 + Views.ToString() + ", "
                                 + Url.ToString() + ", "
                                 + DOI.ToString() + ", "
                                 + JournalReference.ToString() + ")"
                                 + "\nAuthors: ";
            if(AuthorsList != null)
                AuthorsList.ForEach((author) => articleData += "\nID: " + author.AuthorId + "; Name: " + author.AuthorName + "; ");
            articleData += "\nCategories: ";
            if(Categories != null)
                Categories.ForEach((category) => articleData += "\nName: " + category + "; Description: " + Global.Instance.Categories[category] + "; ");

            return articleData;
        }

        /// <summary>
        /// Returns string in tuple format with $myTokken$ for some fields.
        /// </summary>
        /// <returns></returns>
        public string ToSql()
        {
            string articleData = "(" + ArticleId.ToString() + ", $myTokken$"
                                 + Title.ToString() + "$myTokken$, $myTokken$"
                                 + Summary.ToString() + "$myTokken$, "
                                 + Published.ToString("dd.MM.yyyy") + ", "
                                 + Updated.ToString("dd.MM.yyyy") + ", "
                                 + Views.ToString() + ", $myTokken$s"
                                 + Url.ToString() + "$myTokken$, $myTokken$"
                                 + DOI.ToString() + "$myTokken$, $myTokken$"
                                 + JournalReference.ToString() + "$myTokken$)";
            return articleData;
        }

    }

    
}