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
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public JsonResult GetData()
        {
            bool proxyCreation = db.Configuration.ProxyCreationEnabled;
            try
            {
                //set ProxyCreation to false
                db.Configuration.ProxyCreationEnabled = false;

                var data = db.Comments.ToList();

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
        // GET: Comments
        [CustomAuthorize("User", "Contributor", "BlogOwner")]
        public ActionResult Index()
        {
            var comments = db.Comments.Include(c => c.Post);
            return View();
        }

        // GET: Comments/Details/5
        [CustomAuthorize("User", "Contributor", "BlogOwner")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        [CustomAuthorize("BlogOwner")]
        public ActionResult Create()
        {
            ViewBag.PostId = new SelectList(db.Postss, "Id", "Title");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("BlogOwner")]
        public ActionResult Create([Bind(Include = "Id,Name,Email,PostId,CommentHeader,CommentText,CommentTime")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PostId = new SelectList(db.Postss, "Id", "Title", comment.PostId);
            return View(comment);
        }

        // GET: Comments/Edit/5
        [CustomAuthorize("Contributor", "BlogOwner")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.PostId = new SelectList(db.Postss, "Id", "Title", comment.PostId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Contributor", "BlogOwner")]
        public ActionResult Edit([Bind(Include = "Id,Name,Email,PostId,CommentHeader,CommentText,CommentTime")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PostId = new SelectList(db.Postss, "Id", "Title", comment.PostId);
            return View(comment);
        }

        // GET: Comments/Delete/5
        [CustomAuthorize("BlogOwner")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("BlogOwner")]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
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
        public ActionResult UnAuthorized()
        {
            ViewBag.Message = "Un Authorized Page!";
            return View();
        }
    }
}
