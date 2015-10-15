using Microsoft.AspNet.Identity;
using Project_DMD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_DMD.Classes
{
    public sealed class FakeGenerator
    {
        static readonly FakeGenerator _instance = new FakeGenerator();
        
        public static FakeGenerator Instance
        {
            get
            {
                return _instance;
            }
        }

        public FakeDataRepository ArticlesRepository { get; set; }
        public FakeAppUserRepository UsersRepository { get; set; }
        public UserManager<AppUser> UserManager { get; set; }
        private FakeGenerator()
        {
            ArticlesRepository = new FakeDataRepository();
            UsersRepository = new FakeAppUserRepository();
            UserManager = new UserManager<AppUser>(new CustomUserStore());
        }
    }
}