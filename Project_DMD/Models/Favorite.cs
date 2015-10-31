using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project_DMD.Attributes;

namespace Project_DMD.Models
{
    [AgsModel]
    public class Favorite
    {   
        [AgsAttribute]
        public int ArticleId { get; set; }

        [AgsAttribute(Int = true)]
        public string UserId { get; set; }

        [AgsAttribute]
        public DateTime AdditionDate { get; set; }

        [AgsForeign(Name = "ArticleId", TableName = "Article")]
        public Article Article { get; set; }

        [AgsForeign(Name = "UserId",TableName = "Client")]
        public AppUser User { get; set; }
    }
}