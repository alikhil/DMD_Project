using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_DMD.Models
{
    public class Visit
    {
        public int VisitId { get; set; }

        public int ArticleId { get; set; }

        public string UserId { get; set; }

        public DateTime VisitDate { get; set; }

        public Article Article { get; set; }

        public AppUser User { get; set; }
    }
}