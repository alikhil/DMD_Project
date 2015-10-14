using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_DMD.Models
{
    public class ActionHistory
    {
        public int ActionHistoryId { get; set; }
        
        public DateTime ActionDate { get; set; }

        public ActionType ActionDone { get; set; }

        public int ArticleId { get; set; }

        public int UserId { get; set; }

        public AppUser User { get; set; }

        public Article Article { get; set; }
    }

    public enum ActionType
    {
        Add,
        Edit,
        Delete
    }
}