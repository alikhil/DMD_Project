using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_DMD.Models
{
    public class Keyword
    {
        public int KeywordId { get; set; }
        public string KeywordValue { get; set; }

        public List<Article> Articles { get; set; }
    }
}