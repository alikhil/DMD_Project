using Project_DMD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Web.Helpers;
using Project_DMD.Classes;
namespace Project_DMD.Repositories
{
    public class AppUserRepository : IAppUserRepository
    {
        public bool Add(AppUser user)
        {
            try
            {
                QueryExecutor.Instance.AddAppUser(user);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<AppUser> GetAppUsers()
        {
            return QueryExecutor.Instance.GetAppUsers();
        }

        public AppUser GetAppUser(string id)
        {
            return QueryExecutor.Instance.GetAppUser(id);
        }

        public bool UpdateAppUser(AppUser appUser)
        {
            return QueryExecutor.Instance.UpdateAppUser(appUser);
        }


        public void AddFavorite(Favorite favorite)
        {
            QueryExecutor.Instance.AddFavorite(favorite);
        }

        public Favorite FindFavorite(int articleId, string userId)
        {
            return QueryExecutor.Instance.GetFavorite(articleId, userId);
        }

        public void RemoveFavorite(int articleId, string userId)
        {
            QueryExecutor.Instance.RemoveFavorite(articleId, userId);
        }


        public List<Favorite> GetFavorites(string userId)
        {
            return QueryExecutor.Instance.GetFavorites(userId);
        }


        public AppUser GetAppUserByName(string userName)
        {
            return QueryExecutor.Instance.GetAppUserByUserName(userName);
        }


        public void VisitArticle(int articleId, string userId)
        {
            Visit visit = new Visit(){
                ArticleId = articleId,
                UserId = userId,
                VisitDate = DateTime.Now
            };
            QueryExecutor.Instance.CreateVisit(visit);
            Global.Instance.ArticlesRepository.VisitArticle(articleId);
        }

        public List<Visit> GetVisits(string userId)
        {
            return QueryExecutor.Instance.GetVisits(userId);
        }

        public List<ActionHistory> GetActionsForUser(string userId)
        {
            return QueryExecutor.Instance.GetActionsForUser(userId);
        }

        public List<ActionHistory> GetActionsForArticle(int articleId)
        {
            return QueryExecutor.Instance.GetActionsForArticle(articleId);
        }

        public void AddAction(string userId, int articleId, ActionType type)
        {
            var action = MakeAction(userId, articleId, type);
            QueryExecutor.Instance.AddAction(action);
        }

        public static ActionHistory MakeAction(string userId, int articleId, ActionType type)
        {
            return new ActionHistory()
            {
                ArticleId = articleId,
                UserId = userId,
                ActionDate = DateTime.Now,
                ActionDone = type
            };
        }
    }
}