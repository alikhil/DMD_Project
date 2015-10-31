using System;
using Project_DMD.Attributes;


namespace Project_DMD.Models
{
    [AgsModel]
    public class ActionHistory
    {
        [AgsPrimary]
        public int ActionHistoryId { get; set; }
        
        [AgsAttribute]
        public DateTime ActionDate { get; set; }

        [AgsAttribute]
        public ActionType ActionDone { get; set; }

        [AgsAttribute]
        public int ArticleId { get; set; }

        [AgsAttribute(Int = true)]
        public string UserId { get; set; }

        [AgsForeign(Name = "UserId",TableName = "Client")]
        public AppUser User { get; set; }
        [AgsForeign(Name = "ArticleId",TableName = "Article")]
        public Article Article { get; set; }
    }

    public enum ActionType
    {
        Add,
        Edit,
        Delete
    }
}