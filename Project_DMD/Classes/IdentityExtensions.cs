using Project_DMD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet;
using System.Web.Mvc;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
namespace Project_DMD.Classes
{
    public static class IdentityExtensions
    {
        public static AppUser GetAppUser(this IIdentity identity){
            AppUser appUser = null;
            appUser = Global.Instance.UsersRepository.GetAppUserByName(identity.Name);
            return appUser;
        }
    }
}