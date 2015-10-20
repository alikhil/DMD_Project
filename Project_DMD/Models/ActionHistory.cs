using System;


namespace Project_DMD.Models
{
    public class ActionHistory
    {
        public int ActionHistoryId { get; set; }
        
        public DateTime ActionDate { get; set; }

        public ActionType ActionDone { get; set; }

        public int ArticleId { get; set; }

        public string UserId { get; set; }

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