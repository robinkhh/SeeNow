using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SeeNow.Models;

namespace SeeNow.Areas.Backend.Controllers
{
    public class difficulty_levelController : Controller
    {
        private SeeNowEntities db = new SeeNowEntities();

        // GET: difficulty_level
        public ActionResult Index()
        {
            return View(db.difficulty_level.ToList());
        }

        // GET: difficulty_level/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            difficulty_level difficulty_level = db.difficulty_level.Find(id);
            if (difficulty_level == null)
            {
                return HttpNotFound();
            }
            return View(difficulty_level);
        }

        // GET: difficulty_level/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: difficulty_level/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "difficulty_id,difficulty_desc")] difficulty_level difficulty_level)
        {
            if (ModelState.IsValid)
            {
                db.difficulty_level.Add(difficulty_level);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(difficulty_level);
        }

        // GET: difficulty_level/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            difficulty_level difficulty_level = db.difficulty_level.Find(id);
            if (difficulty_level == null)
            {
                return HttpNotFound();
            }
            return View(difficulty_level);
        }

        // POST: difficulty_level/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "difficulty_id,difficulty_desc")] difficulty_level difficulty_level)
        {
            if (ModelState.IsValid)
            {
                db.Entry(difficulty_level).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(difficulty_level);
        }

        // GET: difficulty_level/Delete/5
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            difficulty_level difficulty_level = db.difficulty_level.Find(id);
            if (difficulty_level == null)
            {
                return HttpNotFound();
            }
            return View(difficulty_level);
        }

        // POST: difficulty_level/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            difficulty_level difficulty_level = db.difficulty_level.Find(id);
            db.difficulty_level.Remove(difficulty_level);
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
