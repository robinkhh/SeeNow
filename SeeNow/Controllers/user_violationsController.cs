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
    public class user_violationsController : Controller
    {
        private SeeNowEntities db = new SeeNowEntities();

        // GET: user_violations
        public ActionResult Index()
        {
            var user_violations = db.user_violations.Include(u => u.manager).Include(u => u.users).Include(u => u.users1);
            return View(user_violations.ToList());
        }

        // GET: user_violations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user_violations user_violations = db.user_violations.Find(id);
            if (user_violations == null)
            {
                return HttpNotFound();
            }
            return View(user_violations);
        }

        // GET: user_violations/Create
        public ActionResult Create()
        {
            ViewBag.manager_id = new SelectList(db.manager, "manager_id", "password");
            ViewBag.report_id = new SelectList(db.users, "account", "role_id");
            ViewBag.repoter_id = new SelectList(db.users, "account", "role_id");
            return View();
        }

        // POST: user_violations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "violation_quid,repoter_id,report_id,manager_id,check_flag,datetime,violation_reason")] user_violations user_violations)
        {
            if (ModelState.IsValid)
            {
                db.user_violations.Add(user_violations);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.manager_id = new SelectList(db.manager, "manager_id", "password", user_violations.manager_id);
            ViewBag.report_id = new SelectList(db.users, "account", "role_id", user_violations.report_id);
            ViewBag.repoter_id = new SelectList(db.users, "account", "role_id", user_violations.repoter_id);
            return View(user_violations);
        }

        // GET: user_violations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user_violations user_violations = db.user_violations.Find(id);
            if (user_violations == null)
            {
                return HttpNotFound();
            }
            ViewBag.manager_id = new SelectList(db.manager, "manager_id", "password", user_violations.manager_id);
            ViewBag.report_id = new SelectList(db.users, "account", "role_id", user_violations.report_id);
            ViewBag.repoter_id = new SelectList(db.users, "account", "role_id", user_violations.repoter_id);
            return View(user_violations);
        }

        // POST: user_violations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "violation_quid,repoter_id,report_id,manager_id,check_flag,datetime,violation_reason")] user_violations user_violations)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user_violations).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.manager_id = new SelectList(db.manager, "manager_id", "password", user_violations.manager_id);
            ViewBag.report_id = new SelectList(db.users, "account", "role_id", user_violations.report_id);
            ViewBag.repoter_id = new SelectList(db.users, "account", "role_id", user_violations.repoter_id);
            return View(user_violations);
        }

        // GET: user_violations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user_violations user_violations = db.user_violations.Find(id);
            if (user_violations == null)
            {
                return HttpNotFound();
            }
            return View(user_violations);
        }

        // POST: user_violations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            user_violations user_violations = db.user_violations.Find(id);
            db.user_violations.Remove(user_violations);
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
