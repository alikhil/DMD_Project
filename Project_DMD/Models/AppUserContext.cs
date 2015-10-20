using Project_DMD.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_DMD.Models
{
    public class AppUserContext
    {
        private static AppUserContext instance;

        public static AppUserContext Instance
        {
            get
            {
                if (instance == null)
                    instance = new AppUserContext();
                return instance;
            }
        }

        public List<AppUser> AppUsersList {
            get 
            {
                return Global.Instance.UsersRepository.GetAppUsers();
            } 
        }

        public AppUser Find(string uname)
        {
            return Global.Instance.UsersRepository.GetAppUserByName(uname);
        }

        private AppUserContext()
        {
            
        }

        public bool Add(AppUser user)
        {
            Global.Instance.UsersRepository.Add(user);
            return true;
        }
    }
}