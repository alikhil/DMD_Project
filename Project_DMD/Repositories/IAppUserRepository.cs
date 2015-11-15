using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project_DMD.Models;

namespace Project_DMD.Repositories
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

        List<ActionHistory> GetActionsForUser(string userId);

        List<ActionHistory> GetActionsForArticle(int articleId);

        void AddAction(string userId, int articleId, ActionType type);

    }

}