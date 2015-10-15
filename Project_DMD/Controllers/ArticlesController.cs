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

namespace Project_DMD.Controllers
{
    [Authorize]
    public class ArticlesController : Controller
    {
        private IDataRepository DataRepository = FakeGenerator.Instance.ArticlesRepository;
        private IAppUserRepository UsersRepository = FakeGenerator.Instance.UsersRepository;
        // GET: Articles
        public ActionResult Index()
        {
            return View(DataRepository.GetArticles());
        }

        // GET: Articles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = DataRepository.GetArticle(id.Value);
            if (article == null)
            {
                return HttpNotFound();
            }
            ViewBag.Favorite = UsersRepository.FindFavorite(id.Value, User.Identity.GetAppUser().Id) != null;
            article.Views++;
            UsersRepository.VisitArticle(article.ArticleId, User.Identity.GetAppUser().Id);
            return View(article);
        }

        // GET: Articles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Articles/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Url,Title,Summary,JournalReference,DOI")] Article article)
        {
            if (ModelState.IsValid)
            {
                DataRepository.Add(article);
                return RedirectToAction("Index");
            }

            return View(article);
        }

        // GET: Articles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = DataRepository.GetArticle(id.Value);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ArticleId,Url,Title,Summary,JournalReference,DOI")] Article article)
        {
            if (ModelState.IsValid)
            {
                DataRepository.Update(article);
                return RedirectToAction("Index");
            }
            return View(article);
        }

        // GET: Articles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = DataRepository.GetArticle(id.Value);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Article article = DataRepository.GetArticle(id);
            DataRepository.Delete(id);
            return RedirectToAction("Index");
        }

        public ActionResult ShowAuthor(int id)
        {
            if (id != null)
            {
                return View(DataRepository.GetAuthor(id));
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //Repository.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        public ActionResult Favorite(int articleId)
        {
            if (DataRepository.GetArticle(articleId) != null)
            {
                var curAppUser = User.Identity.GetAppUser();
                Favorite fav = new Favorite()
                {
                    ArticleId = articleId,
                    UserId = curAppUser.Id,
                    AdditionDate = DateTime.Now
                };
                if (UsersRepository.FindFavorite(articleId, curAppUser.Id) == null)
                    UsersRepository.AddFavorite(fav);
            }
            return RedirectToAction("Details", "Articles", new { id = articleId });
        }
    }
}
