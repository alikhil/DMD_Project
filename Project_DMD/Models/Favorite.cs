using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_DMD.Models
{
    public class Favorite
    {
        public int ArticleId { get; set; }

        public int UserId { get; set; }

        public DateTime AdditionDate { get; set; }

        public Article Article { get; set; }

        public Client User { get; set; }
    }
}