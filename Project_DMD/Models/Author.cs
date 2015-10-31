using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project_DMD.Attributes;

namespace Project_DMD.Models
{
    [AgsModel]
    public class Author
    {
        /// <summary>
        /// Author id
        /// </summary>
        [AgsPrimary]
        public int AuthorId { get; set; }

        /// <summary>
        /// Author's Name and Last Name
        /// </summary>
        [AgsAttribute]
        public string AuthorName { get; set; }

        /// <summary>
        /// Articles published by this author
        /// </summary>
        [AgsForeign(Name = "AuthorId", TableName = "ArticleAuthors")]
        public ICollection<Article> PublishedArticles { get; set; }

        public Author() { }

        public Author(int authorId, string authorName)
        {
            AuthorId = authorId;
            AuthorName = authorName;
        }
    }
}