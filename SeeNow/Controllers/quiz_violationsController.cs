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
    public class quiz_violationsController : Controller
    {
        private SeeNowEntities db = new SeeNowEntities();

        // GET: quiz_violations
        public ActionResult Index()
        {
            var quiz_violations = db.quiz_violations.Include(q => q.manager).Include(q => q.quizzes).Include(q => q.users);
            return View(quiz_violations.ToList());
        }

        // GET: quiz_violations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            quiz_violations quiz_violations = db.quiz_violations.Find(id);
            if (quiz_violations == null)
            {
                return HttpNotFound();
            }
            return View(quiz_violations);
        }

        // GET: quiz_violations/Create
        public ActionResult Create()
        {
            ViewBag.manager_id = new SelectList(db.manager, "manager_id", "password");
            ViewBag.quiz_guid = new SelectList(db.quizzes, "quiz_guid", "type_id");
            ViewBag.reporter_id = new SelectList(db.users, "account", "role_id");
            return View();
        }

        // POST: quiz_violations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "violation_quid,reporter_id,quiz_guid,manager_id,check_flag,datetime,violation_reason")] quiz_violations quiz_violations)
        {
            if (ModelState.IsValid)
            {
                db.quiz_violations.Add(quiz_violations);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.manager_id = new SelectList(db.manager, "manager_id", "password", quiz_violations.manager_id);
            ViewBag.quiz_guid = new SelectList(db.quizzes, "quiz_guid", "type_id", quiz_violations.quiz_guid);
            ViewBag.reporter_id = new SelectList(db.users, "account", "role_id", quiz_violations.reporter_id);
            return View(quiz_violations);
        }

        // GET: quiz_violations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            quiz_violations quiz_violations = db.quiz_violations.Find(id);
            if (quiz_violations == null)
            {
                return HttpNotFound();
            }
            ViewBag.manager_id = new SelectList(db.manager, "manager_id", "password", quiz_violations.manager_id);
            ViewBag.quiz_guid = new SelectList(db.quizzes, "quiz_guid", "type_id", quiz_violations.quiz_guid);
            ViewBag.reporter_id = new SelectList(db.users, "account", "role_id", quiz_violations.reporter_id);
            return View(quiz_violations);
        }

        // POST: quiz_violations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "violation_quid,reporter_id,quiz_guid,manager_id,check_flag,datetime,violation_reason")] quiz_violations quiz_violations)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quiz_violations).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.manager_id = new SelectList(db.manager, "manager_id", "password", quiz_violations.manager_id);
            ViewBag.quiz_guid = new SelectList(db.quizzes, "quiz_guid", "type_id", quiz_violations.quiz_guid);
            ViewBag.reporter_id = new SelectList(db.users, "account", "role_id", quiz_violations.reporter_id);
            return View(quiz_violations);
        }

        // GET: quiz_violations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            quiz_violations quiz_violations = db.quiz_violations.Find(id);
            if (quiz_violations == null)
            {
                return HttpNotFound();
            }
            return View(quiz_violations);
        }

        // POST: quiz_violations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            quiz_violations quiz_violations = db.quiz_violations.Find(id);
            db.quiz_violations.Remove(quiz_violations);
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
