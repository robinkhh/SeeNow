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
    public class play_record_answerController : Controller
    {
        private SeeNowEntities db = new SeeNowEntities();

        // GET: play_record_answer
        public ActionResult Index()
        {
            var play_record_answer = db.play_record_answer.Include(p => p.play_record);
            return View(play_record_answer.ToList());
        }

        // GET: play_record_answer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            play_record_answer play_record_answer = db.play_record_answer.Find(id);
            if (play_record_answer == null)
            {
                return HttpNotFound();
            }
            return View(play_record_answer);
        }

        // GET: play_record_answer/Create
        public ActionResult Create()
        {
            ViewBag.play_guid = new SelectList(db.play_record, "play_guid", "account");
            return View();
        }

        // POST: play_record_answer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "play_guid,account,select_answer,quiz_guid")] play_record_answer play_record_answer)
        {
            if (ModelState.IsValid)
            {
                db.play_record_answer.Add(play_record_answer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.play_guid = new SelectList(db.play_record, "play_guid", "account", play_record_answer.play_guid);
            return View(play_record_answer);
        }

        // GET: play_record_answer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            play_record_answer play_record_answer = db.play_record_answer.Find(id);
            if (play_record_answer == null)
            {
                return HttpNotFound();
            }
            ViewBag.play_guid = new SelectList(db.play_record, "play_guid", "account", play_record_answer.play_guid);
            return View(play_record_answer);
        }

        // POST: play_record_answer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "play_guid,account,select_answer,quiz_guid")] play_record_answer play_record_answer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(play_record_answer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.play_guid = new SelectList(db.play_record, "play_guid", "account", play_record_answer.play_guid);
            return View(play_record_answer);
        }

        // GET: play_record_answer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            play_record_answer play_record_answer = db.play_record_answer.Find(id);
            if (play_record_answer == null)
            {
                return HttpNotFound();
            }
            return View(play_record_answer);
        }

        // POST: play_record_answer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            play_record_answer play_record_answer = db.play_record_answer.Find(id);
            db.play_record_answer.Remove(play_record_answer);
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
