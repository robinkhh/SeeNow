using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SeeNow.Models;

namespace SeeNow.Controllers
{
    public class PostsController : Controller
    {
        private SeeNowEntities db = new SeeNowEntities();

        // GET: Posts
        public ActionResult Index()
        {
            var post = db.post.Include(p => p.users).Include(p => p.quizzes);
            return View(post.ToList());
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            post post = db.post.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            ViewBag.account = new SelectList(db.users, "account", "role_id");
            ViewBag.quiz_guid = new SelectList(db.quizzes, "quiz_guid", "type_id");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "post_quid,quiz_guid,content,datetime,lock_flag,account")] post post)
        {
            if (ModelState.IsValid)
            {
                db.post.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.account = new SelectList(db.users, "account", "role_id", post.account);
            ViewBag.quiz_guid = new SelectList(db.quizzes, "quiz_guid", "type_id", post.quiz_guid);
            return View(post);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            post post = db.post.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.account = new SelectList(db.users, "account", "role_id", post.account);
            ViewBag.quiz_guid = new SelectList(db.quizzes, "quiz_guid", "type_id", post.quiz_guid);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "post_quid,quiz_guid,content,datetime,lock_flag,account")] post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.account = new SelectList(db.users, "account", "role_id", post.account);
            ViewBag.quiz_guid = new SelectList(db.quizzes, "quiz_guid", "type_id", post.quiz_guid);
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            post post = db.post.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            post post = db.post.Find(id);
            db.post.Remove(post);
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
    }
}
