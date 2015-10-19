using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project_DMD.Models;
using Project_DMD.Classes;
using Microsoft.AspNet.Identity;

namespace Project_DMD.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        UserManager<AppUser> userManager = FakeGenerator.Instance.UserManager;
        IAppUserRepository UsersRepository = FakeGenerator.Instance.UsersRepository;
        public ActionResult Index()
        {
            var curAppUser = User.Identity.GetAppUser();
            return View(curAppUser);
        }

       

        public ActionResult Edit()
        {
            var appUser = User.Identity.GetAppUser();
            var tmp = new AppUser()
                {
                    FirstName = appUser.FirstName,
                    LastName = appUser.LastName
                };
            return View(tmp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FirstName,LastName")] AppUser appUser)
        {
            var curAppUser = User.Identity.GetAppUser();
            appUser.Id = curAppUser.Id;
            appUser.UserName = curAppUser.UserName;
            appUser.Email = curAppUser.Email;
            appUser.PasswordHash = curAppUser.PasswordHash;

            if (ModelState.IsValid)
            {
               
                UsersRepository.UpdateAppUser(appUser);
                return RedirectToAction("Index");
            }
            return View(appUser);
        }

        public ActionResult Favorites()
        {
            var appUser = User.Identity.GetAppUser();
            var favorites = UsersRepository.GetFavorites(appUser.Id);
            return View(favorites);
        }
    }
}
