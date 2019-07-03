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
    public class quiz_answerController : Controller
    {
        private SeeNowEntities db = new SeeNowEntities();

        // GET: quiz_answer
        public ActionResult Index()
        {
            var quiz_answer = db.quiz_answer.Include(q => q.quizzes).Include(q => q.type);
            return View(quiz_answer.ToList());
        }

        // GET: quiz_answer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            quiz_answer quiz_answer = db.quiz_answer.Find(id);
            if (quiz_answer == null)
            {
                return HttpNotFound();
            }
            return View(quiz_answer);
        }

        // GET: quiz_answer/Create
        public ActionResult Create()
        {
            ViewBag.quiz_guid = new SelectList(db.quizzes, "quiz_guid", "type_id");
            ViewBag.type_id = new SelectList(db.type, "type_id", "type_desc");
            return View();
        }

        // POST: quiz_answer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "quiz_guid,answer_id,type_id,answer_text,answer_img_path,is_correct")] quiz_answer quiz_answer)
        {
            if (ModelState.IsValid)
            {
                db.quiz_answer.Add(quiz_answer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.quiz_guid = new SelectList(db.quizzes, "quiz_guid", "type_id", quiz_answer.quiz_guid);
            ViewBag.type_id = new SelectList(db.type, "type_id", "type_desc", quiz_answer.type_id);
            return View(quiz_answer);
        }

        // GET: quiz_answer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            quiz_answer quiz_answer = db.quiz_answer.Find(id);
            if (quiz_answer == null)
            {
                return HttpNotFound();
            }
            ViewBag.quiz_guid = new SelectList(db.quizzes, "quiz_guid", "type_id", quiz_answer.quiz_guid);
            ViewBag.type_id = new SelectList(db.type, "type_id", "type_desc", quiz_answer.type_id);
            return View(quiz_answer);
        }

        // POST: quiz_answer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "quiz_guid,answer_id,type_id,answer_text,answer_img_path,is_correct")] quiz_answer quiz_answer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quiz_answer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.quiz_guid = new SelectList(db.quizzes, "quiz_guid", "type_id", quiz_answer.quiz_guid);
            ViewBag.type_id = new SelectList(db.type, "type_id", "type_desc", quiz_answer.type_id);
            return View(quiz_answer);
        }

        // GET: quiz_answer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            quiz_answer quiz_answer = db.quiz_answer.Find(id);
            if (quiz_answer == null)
            {
                return HttpNotFound();
            }
            return View(quiz_answer);
        }

        // POST: quiz_answer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            quiz_answer quiz_answer = db.quiz_answer.Find(id);
            db.quiz_answer.Remove(quiz_answer);
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
