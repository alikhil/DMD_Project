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
                return FakeGenerator.Instance.UsersRepository.GetAppUsers();
            } 
        }

        public AppUser Find(string uname)
        {
            return FakeGenerator.Instance.UsersRepository.GetAppUserByName(uname);
        }

        private AppUserContext()
        {
            
        }

        public bool Add(AppUser user)
        {
            FakeGenerator.Instance.UsersRepository.Add(user);
            return true;
        }
    }
}