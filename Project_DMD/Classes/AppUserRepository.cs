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

        bool UpdateAppUser(AppUser appUser);
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
    }

    public class FakeAppUserRepository : IAppUserRepository
    {
        private List<AppUser> Users { get; set; }

        public FakeAppUserRepository()
        {
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
    }
}