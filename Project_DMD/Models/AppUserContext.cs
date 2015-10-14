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

        public List<AppUser> DummyUsersList { get; set; }

        private AppUserContext()
        {
            DummyUsersList = new List<AppUser>();
        }

        public bool Add(AppUser user)
        {
            DummyUsersList.Add(user);
            return true;
        }
    }
}