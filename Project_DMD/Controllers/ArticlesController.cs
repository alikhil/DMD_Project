using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Project_DMD.Models;
using Project_DMD.Classes;
using Project_DMD.Repositories;
using System.Linq;
using Project_DMD.Classes.Extensions;
namespace Project_DMD.Controllers
{
    [Authorize]
    public class ArticlesController : Controller
    {
        readonly IDataRepository DataRepository = Global.Instance.ArticlesRepository;
        readonly IAppUserRepository UsersRepository = Global.Instance.UsersRepository;
        
        public ActionResult Index([Bind(Exclude = "Articles, ElapsedTime")]ArticlesIndexViewModel model)
        {
            Stopwatch watch = new Stopwatch();
            var tempDict = Global.Instance.InitCategories();
            tempDict.Add("","");
            ViewBag.SelectedList = new SelectList(tempDict, "Key", "Value", model.CategoryName);
            ViewBag.Sortes = EnumExtensions.ToSelectList<SortTypeEnum>(); 
            watch.Start();
            model.Articles = DataRepository.GetArticles(model.Page, model.AuthorName, model.ArticleSummary, model.AuthorName, model.PublicationYear, model.CategoryName,
                model.JournalReference, (int)model.SortType, model.OrderByDescending);
            model.ElapsedTime= watch.Elapsed.TotalMilliseconds;
            return View(model);
        }

        public JsonResult GetAuthors(string search)
        {
            var result = DataRepository.GetAuthorsByName(search);
            return Json(result, JsonRequestBehavior.AllowGet);
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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Url,Title,Summary,JournalReference,DOI,Categories,Authors")] Article article)
        {

            if (ModelState.IsValid && article.Authors != null && article.Authors.Count > 0)
            {
                article.ParseAuthors();
                var articleId = DataRepository.Add(article);
                UsersRepository.AddAction(User.Identity.GetUserId(), articleId, ActionType.Add);
                return RedirectToAction("Index");
            }
            if (article.Authors == null || article.Authors.Count == 0)
                ModelState.AddModelError("Authors","Select authors!");
            ViewBag.SelectedList = new SelectList(Global.Instance.Categories, "Key", "Value");
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

        public ActionResult RemoveFromFavorite(int? articleId)
        {
            var userId = User.Identity.GetUserId();
            if(articleId != null && !userId.IsNullOrWhiteSpace())
                UsersRepository.RemoveFavorite(articleId.Value, userId);
            return RedirectToAction("Favorites", "Manage");
        }
    }
}
