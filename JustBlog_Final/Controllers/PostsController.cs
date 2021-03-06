using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using JustBlog_Final.Infrastructure;
using JustBlog_Final.Models;

namespace JustBlog_Final.Controllers
{
    [CustomAuthenticationFilter]
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public JsonResult GetData()
        {
            bool proxyCreation = db.Configuration.ProxyCreationEnabled;
            try
            {
                //set ProxyCreation to false
                db.Configuration.ProxyCreationEnabled = false;

                var data = db.Postss.ToList();

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
            finally
            {
                //restore ProxyCreation to its original state
                db.Configuration.ProxyCreationEnabled = proxyCreation;
            }
        }
        // GET: Posts
        [CustomAuthorize("User", "Contributor", "BlogOwner")]
        public ActionResult Index()
        {
            var postss = db.Postss.Include(p => p.Category);
            return View(postss.ToList());
        }

        // GET: Posts/Details/5
        [CustomAuthorize("User", "Contributor", "BlogOwner")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Postss.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Posts/Create
        [CustomAuthorize("Contributor", "BlogOwner")]
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [CustomAuthorize("Contributor", "BlogOwner")]
        public ActionResult Create([Bind(Include = "Id,Title,ShortDescription,PostContent,UrlSlug,Published,PostedOn,Modified,CategoryId,ViewCount")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Postss.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", post.CategoryId);
            return View(post);
        }

        // GET: Posts/Edit/5
        [CustomAuthorize("Contributor", "BlogOwner")]

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Postss.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", post.CategoryId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [CustomAuthorize("Contributor", "BlogOwner")]
        public ActionResult Edit([Bind(Include = "Id,Title,ShortDescription,PostContent,UrlSlug,Published,PostedOn,Modified,CategoryId,ViewCount")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", post.CategoryId);
            return View(post);
        }

        // GET: Posts/Delete/5
        [CustomAuthorize("BlogOwner")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Postss.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("BlogOwner")]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Postss.Find(id);
            db.Postss.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [ChildActionOnly]
        public ActionResult LastestPosts()
        {
            using (var _context = new ApplicationDbContext())
            {
                var lastestPost = _context.Postss.OrderByDescending(p => p.PostedOn).ToList();
                if (lastestPost.Count >= 5)
                {
                    lastestPost = lastestPost.Take(5).ToList();
                }
                return View("_ListPosts", lastestPost);
            }
        }

        [ChildActionOnly]
        public ActionResult MostViewPost()
        {
            var mostView = db.Postss.OrderByDescending(p => p.ViewCount).ToList();
            if (mostView.Count > 0)
            {
                return View("_ListPosts", mostView);
            }
            else
            {
                return HttpNotFound();
            }
        }
        [ChildActionOnly]
        public ActionResult PublishedPosts()
        {
            var mostView = db.Postss.Any(p => p.Published == true);
            return View("_ListPosts", mostView);
        }
        public ActionResult UnAuthorized()
        {
            ViewBag.Message = "Un Authorized Page!";
            return View();
        }
    }
}
