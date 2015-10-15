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
        UserManager<AppUser> userManager = new UserManager<AppUser>(new CustomUserStore());
        IAppUserRepository UsersRepository = FakeGenerator.Instance.UsersRepository;
        public ActionResult Index()
        {
            var curAppUser = userManager.FindByName(User.Identity.Name);
            return View(curAppUser);
        }

       

        public ActionResult Edit()
        {
            var appUser = userManager.FindByName(User.Identity.Name);
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
            var curAppUser = userManager.FindByName(User.Identity.Name);
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


        public ActionResult Favorite(int articleId)
        {
            var curAppUser = userManager.FindByName(User.Identity.Name);
            Favorite fav = new Favorite()
            {
                ArticleId = articleId,
                UserId = curAppUser.Id,
                AdditionDate = DateTime.Now
            };
            if(UsersRepository.FindFavorite(articleId, curAppUser.Id) == null)
                UsersRepository.AddFavorite(fav);
            return RedirectToAction("Details", "Articles", new { id = articleId });
        }
      
    }
}
