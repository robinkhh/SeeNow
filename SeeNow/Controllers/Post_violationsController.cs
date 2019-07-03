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
    public class Post_violationsController : Controller
    {
        private SeeNowEntities db = new SeeNowEntities();

        // GET: Post_violations
        public ActionResult Index()
        {
            var post_violations = db.post_violations.Include(p => p.manager).Include(p => p.post).Include(p => p.users);
            return View(post_violations.ToList());
        }

        // GET: Post_violations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            post_violations post_violations = db.post_violations.Find(id);
            if (post_violations == null)
            {
                return HttpNotFound();
            }
            return View(post_violations);
        }

        // GET: Post_violations/Create
        public ActionResult Create()
        {
            ViewBag.manager_id = new SelectList(db.manager, "manager_id", "password");
            ViewBag.post_quid = new SelectList(db.post, "post_quid", "content");
            ViewBag.reporter_id = new SelectList(db.users, "account", "role_id");
            return View();
        }

        // POST: Post_violations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "violation_quid,reporter_id,post_quid,manager_id,check_flag,datetime,violation_reason")] post_violations post_violations)
        {
            if (ModelState.IsValid)
            {
                db.post_violations.Add(post_violations);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.manager_id = new SelectList(db.manager, "manager_id", "password", post_violations.manager_id);
            ViewBag.post_quid = new SelectList(db.post, "post_quid", "content", post_violations.post_quid);
            ViewBag.reporter_id = new SelectList(db.users, "account", "role_id", post_violations.reporter_id);
            return View(post_violations);
        }

        // GET: Post_violations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            post_violations post_violations = db.post_violations.Find(id);
            if (post_violations == null)
            {
                return HttpNotFound();
            }
            ViewBag.manager_id = new SelectList(db.manager, "manager_id", "password", post_violations.manager_id);
            ViewBag.post_quid = new SelectList(db.post, "post_quid", "content", post_violations.post_quid);
            ViewBag.reporter_id = new SelectList(db.users, "account", "role_id", post_violations.reporter_id);
            return View(post_violations);
        }

        // POST: Post_violations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "violation_quid,reporter_id,post_quid,manager_id,check_flag,datetime,violation_reason")] post_violations post_violations)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post_violations).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.manager_id = new SelectList(db.manager, "manager_id", "password", post_violations.manager_id);
            ViewBag.post_quid = new SelectList(db.post, "post_quid", "content", post_violations.post_quid);
            ViewBag.reporter_id = new SelectList(db.users, "account", "role_id", post_violations.reporter_id);
            return View(post_violations);
        }

        // GET: Post_violations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            post_violations post_violations = db.post_violations.Find(id);
            if (post_violations == null)
            {
                return HttpNotFound();
            }
            return View(post_violations);
        }

        // POST: Post_violations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            post_violations post_violations = db.post_violations.Find(id);
            db.post_violations.Remove(post_violations);
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
