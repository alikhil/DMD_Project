using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Project_DMD.Models;
using Project_DMD.Classes;

namespace Project_DMD.Controllers
{
    [Authorize]
    public class ArticlesController : Controller
    {
        readonly IDataRepository DataRepository = Global.Instance.ArticlesRepository;
        readonly IAppUserRepository UsersRepository = Global.Instance.UsersRepository;
        
        public ActionResult Index(string articleName = null, string keyword = null, string authorName = null, int publicationYear = 0, string category = null)
        {
            var tempDict = Global.Instance.InitCategories();
            tempDict.Add("","");
            ViewBag.SelectedList = new SelectList(tempDict, "Key", "Value");
            return View(DataRepository.GetArticles(articleName, keyword, authorName, publicationYear, category));
        }

        
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
            ViewBag.Favorite = UsersRepository.FindFavorite(id.Value, User.Identity.GetUserId()) != null;
            article.Views++;
            UsersRepository.VisitArticle(article.ArticleId, User.Identity.GetUserId());
            return View(article);
        }

       
        public ActionResult Create()
        {
            ViewBag.SelectedList = new SelectList(Global.Instance.Categories, "Key", "Value");
            ViewBag.Authors = new SelectList(Global.Instance.ArticlesRepository.GetAuthors(),"AuthorId", "AuthorName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Url,Title,Summary,JournalReference,DOI,Categories,Authors")] Article article)
        {
            
            if (ModelState.IsValid)
            {
                article.ParseAuthors();
                var articleId = DataRepository.Add(article);
                UsersRepository.AddAction(User.Identity.GetUserId(), articleId, ActionType.Add);
                return RedirectToAction("Index");
            }
            ViewBag.SelectedList = new SelectList(Global.Instance.Categories, "Key", "Value");
            ViewBag.Authors = new SelectList(Global.Instance.ArticlesRepository.GetAuthors(), "AuthorId", "AuthorName");
            return View(article);
        }

        
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
            ViewBag.SelectedList = new SelectList(Global.Instance.Categories, "Key", "Value");
            return View(article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ArticleId,Url,Title,Summary,JournalReference,DOI,Categories,Authors")] Article article)
        {
            if (ModelState.IsValid)
            {   
                DataRepository.Update(article);
                UsersRepository.AddAction(User.Identity.GetUserId(),article.ArticleId, ActionType.Edit);
                return RedirectToAction("Index");
            }
            ViewBag.SelectedList = new SelectList(Global.Instance.Categories, "Key", "Value");
            return View(article);
        }

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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (DataRepository.GetArticle(id.Value) == null)
                return RedirectToAction("Index");
            DataRepository.Delete(id.Value);
            UsersRepository.AddAction(User.Identity.GetUserId(),id.Value, ActionType.Delete);
            return RedirectToAction("Index");
        }

        public ActionResult ShowAuthor(int? id)
        {
            if (id != null)
            {
                return View(DataRepository.GetAuthor(id.Value));
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
            if (DataRepository.GetArticle(articleId) == null)
                return RedirectToAction("Details", "Articles", new {id = articleId});

            var curAppUserId = User.Identity.GetUserId();
            Favorite fav = new Favorite()
            {
                ArticleId = articleId,
                UserId = curAppUserId,
                AdditionDate = DateTime.Now
            };
            if (UsersRepository.FindFavorite(articleId, curAppUserId) == null)
                UsersRepository.AddFavorite(fav);
            return RedirectToAction("Details", "Articles", new { id = articleId });
        }

        public ActionResult RemoveFromFavorite(int? articleId, string userId)
        {
            if(articleId != null && !userId.IsNullOrWhiteSpace())
                UsersRepository.RemoveFavorite(articleId.Value, userId);
            return RedirectToAction("Favorites", "Manage");
        }
    }
}
