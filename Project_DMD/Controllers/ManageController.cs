
using System.Web.Mvc;
using Project_DMD.Models;
using Project_DMD.Classes;
using Microsoft.AspNet.Identity;
using Project_DMD.Repositories;

namespace Project_DMD.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        UserManager<AppUser> userManager = Global.Instance.UserManager;
        IAppUserRepository UsersRepository = Global.Instance.UsersRepository;
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
            var userId = User.Identity.GetUserId();
            var favorites = UsersRepository.GetFavorites(userId);
            return View(favorites);
        }

        public ActionResult Visits()
        {
            var userId = User.Identity.GetUserId();
            var visits = UsersRepository.GetVisits(userId);
            return View(visits);
        }

        public ActionResult Actions()
        {
            var userId = User.Identity.GetUserId();
            var actions = Global.Instance.UsersRepository.GetActionsForUser(userId);
            return View(actions);
        }
    }
}
