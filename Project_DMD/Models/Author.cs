using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_DMD.Models
{
    public class Author
    {
        /// <summary>
        /// Author id
        /// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        /// Author's Name and Last Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Articles published by this author
        /// </summary>
        public ICollection<Article> PublishedArticles { get; set; }
    }
}