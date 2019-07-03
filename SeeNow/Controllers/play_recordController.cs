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
    public class play_recordController : Controller
    {
        private SeeNowEntities db = new SeeNowEntities();

        // GET: play_record
        public ActionResult Index()
        {
            var play_record = db.play_record.Include(p => p.users).Include(p => p.quizzes).Include(p => p.play_record_answer);
            return View(play_record.ToList());
        }

        // GET: play_record/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            play_record play_record = db.play_record.Find(id);
            if (play_record == null)
            {
                return HttpNotFound();
            }
            return View(play_record);
        }

        // GET: play_record/Create
        public ActionResult Create()
        {
            ViewBag.account = new SelectList(db.users, "account", "role_id");
            ViewBag.quiz_guid = new SelectList(db.quizzes, "quiz_guid", "type_id");
            ViewBag.play_guid = new SelectList(db.play_record_answer, "play_guid", "account");
            return View();
        }

        // POST: play_record/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "play_guid,account,score,datetime,quiz_guid")] play_record play_record)
        {
            if (ModelState.IsValid)
            {
                db.play_record.Add(play_record);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.account = new SelectList(db.users, "account", "role_id", play_record.account);
            ViewBag.quiz_guid = new SelectList(db.quizzes, "quiz_guid", "type_id", play_record.quiz_guid);
            ViewBag.play_guid = new SelectList(db.play_record_answer, "play_guid", "account", play_record.play_guid);
            return View(play_record);
        }

        // GET: play_record/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            play_record play_record = db.play_record.Find(id);
            if (play_record == null)
            {
                return HttpNotFound();
            }
            ViewBag.account = new SelectList(db.users, "account", "role_id", play_record.account);
            ViewBag.quiz_guid = new SelectList(db.quizzes, "quiz_guid", "type_id", play_record.quiz_guid);
            ViewBag.play_guid = new SelectList(db.play_record_answer, "play_guid", "account", play_record.play_guid);
            return View(play_record);
        }

        // POST: play_record/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "play_guid,account,score,datetime,quiz_guid")] play_record play_record)
        {
            if (ModelState.IsValid)
            {
                db.Entry(play_record).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.account = new SelectList(db.users, "account", "role_id", play_record.account);
            ViewBag.quiz_guid = new SelectList(db.quizzes, "quiz_guid", "type_id", play_record.quiz_guid);
            ViewBag.play_guid = new SelectList(db.play_record_answer, "play_guid", "account", play_record.play_guid);
            return View(play_record);
        }

        // GET: play_record/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            play_record play_record = db.play_record.Find(id);
            if (play_record == null)
            {
                return HttpNotFound();
            }
            return View(play_record);
        }

        // POST: play_record/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            play_record play_record = db.play_record.Find(id);
            db.play_record.Remove(play_record);
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
