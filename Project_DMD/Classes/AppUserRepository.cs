using Project_DMD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_DMD.Classes
{
    public interface IAppUserRepository
    {
        bool Add(AppUser user);

        List<AppUser> GetAppUsers();
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
    }
}