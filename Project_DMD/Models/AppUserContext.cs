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

        public List<AppUser> DummyUsersList {
            get 
            {
                return FakeGenerator.Instance.FakeUsersRepository.GetAppUsers();
            } 
        }

        private AppUserContext()
        {
            
        }

        public bool Add(AppUser user)
        {
            FakeGenerator.Instance.FakeUsersRepository.Add(user);
            return true;
        }
    }
}