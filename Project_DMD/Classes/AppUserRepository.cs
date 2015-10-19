using Project_DMD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace Project_DMD.Classes
{
    public interface IAppUserRepository
    {
        bool Add(AppUser user);

        List<AppUser> GetAppUsers();

        AppUser GetAppUser(string id);

        AppUser GetAppUserByName(string userName);

        bool UpdateAppUser(AppUser appUser);

        void AddFavorite(Favorite favorite);

        Favorite FindFavorite(int articleId, string userId);

        void RemoveFavorite(int articleId, string userId);

        List<Favorite> GetFavorites(string userId);

        void VisitArticle(int articleId, string userId);

        List<Visit> GetVisits(string p);
    }

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
            FakeGenerator.Instance.ArticlesRepository.VisitArticle(articleId);
        }

        public List<Visit> GetVisits(string userId)
        {
            return QueryExecutor.Instance.GetVisits(userId);
        }
    }

    public class FakeAppUserRepository : IAppUserRepository
    {
        private List<AppUser> Users { get; set; }
        private List<Favorite> Favorites { get; set; }
        private List<Visit> Visits { get; set; }

        public FakeAppUserRepository()
        {
            Favorites = new List<Favorite>();
            Visits = new List<Visit>();
            #region Generate fake data
            Users = new List<AppUser>(new []{ 
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
                result.Article = result.Article ?? FakeGenerator.Instance.ArticlesRepository.GetArticle(articleId);
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
                VisitDate = DateTime.Now
            };
            Visits.Add(visit);
            FakeGenerator.Instance.ArticlesRepository.VisitArticle(articleId);
        }

        public List<Visit> GetVisits(string userId)
        {
            return Visits.FindAll((x => x.UserId == userId));
        }
    }
}