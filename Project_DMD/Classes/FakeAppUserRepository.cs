using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using Project_DMD.Models;

namespace Project_DMD.Classes
{
    public class FakeAppUserRepository : IAppUserRepository
    {
        private List<AppUser> Users { get; set; }
        private List<Favorite> Favorites { get; set; }
        private List<Visit> Visits { get; set; }
        private List<ActionHistory> Actions { get; set; }

        public FakeAppUserRepository()
        {
            Favorites = new List<Favorite>();
            Actions = new List<ActionHistory>();
            Visits = new List<Visit>();
            #region Generate fake data
            Users = new List<AppUser>(new[]{ 
                new AppUser()
                {
                    Email = "ivan@jurov.df",
                    FirstName = "Ivan",
                    LastName = "Jurov",
                    Id = (Guid.NewGuid()).ToString(),
                    UserName = "ivan@jurov.df",
                    PasswordHash = Crypto.HashPassword("QWEqwe123!@#")
                },
                new AppUser()
                {
                    Email = "kolyazn@erk.ov",
                    FirstName = "Kontantin",
                    LastName = "Erkov",
                    Id = (Guid.NewGuid()).ToString(),
                    UserName = "kolyazn@erk.ov",
                    PasswordHash = Crypto.HashPassword("QWEqwe123!@#")
                }
            });
            #endregion
        }
        public bool Add(AppUser user)
        {
            user.UserName = user.Email;
            Users.Add(user);
            return true;
        }

        public List<AppUser> GetAppUsers()
        {
            return Users;
        }

        public AppUser GetAppUser(string id)
        {
            return Users.Find(x => x.Id == id);
        }

        public bool UpdateAppUser(AppUser appUser)
        {
            var curAppUser = GetAppUser(appUser.Id);
            curAppUser.FirstName = appUser.FirstName;
            curAppUser.LastName = appUser.LastName;
            return true;
        }


        public void AddFavorite(Favorite favorite)
        {
            Favorites.Add(favorite);
        }

        public Favorite FindFavorite(int articleId, string userId)
        {
            var result = Favorites.Find(x => x.ArticleId == articleId && x.UserId == userId);
            if (result != null)
            {
                result.User = result.User ?? Users.Find(x => x.Id == userId);
                result.Article = result.Article ?? Global.Instance.ArticlesRepository.GetArticle(articleId);
            }
            return result;
        }

        public void RemoveFavorite(int articleId, string userId)
        {
            var favorite = Favorites.Find(x => x.ArticleId == articleId && x.UserId == userId);
            Favorites.Remove(favorite);
        }

        public List<Favorite> GetFavorites(string userId)
        {
            return Favorites.FindAll(x => x.UserId == userId);
        }


        public AppUser GetAppUserByName(string userName)
        {
            return Users.Find(x => x.Email == userName);
        }


        public void VisitArticle(int articleId, string userId)
        {
            Visit visit = new Visit()
            {
                ArticleId = articleId,
                UserId = userId,
                VisitDate = DateTime.Now,
                Article = Global.Instance.ArticlesRepository.GetArticle(articleId),
                User = GetAppUser(userId)
            };
            Visits.Add(visit);
            Global.Instance.ArticlesRepository.VisitArticle(articleId);
        }

        public List<Visit> GetVisits(string userId)
        {
            return Visits.FindAll((x => x.UserId == userId));
        }


        public List<ActionHistory> GetActionsForUser(string userId)
        {
            return Actions.FindAll(x => x.UserId == userId);
        }

        public List<ActionHistory> GetActionsForArticle(int articleId)
        {
            return Actions.FindAll(x => x.ArticleId == articleId);
        }

        public void AddAction(string userId, int articleId, ActionType type)
        {
            Actions.Add(AppUserRepository.MakeAction(userId, articleId, type));

        }
    }

}