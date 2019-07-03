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
    public class difficulty_level_logController : Controller
    {
        private SeeNowEntities db = new SeeNowEntities();

        // GET: difficulty_level_log
        public ActionResult Index()
        {
            var difficulty_level_log = db.difficulty_level_log.Include(d => d.difficulty_level).Include(d => d.manager);
            return View(difficulty_level_log.ToList());
        }

        // GET: difficulty_level_log/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            difficulty_level_log difficulty_level_log = db.difficulty_level_log.Find(id);
            if (difficulty_level_log == null)
            {
                return HttpNotFound();
            }
            return View(difficulty_level_log);
        }

        // GET: difficulty_level_log/Create
        public ActionResult Create()
        {
            ViewBag.difficulty_id = new SelectList(db.difficulty_level, "difficulty_id", "difficulty_desc");
            ViewBag.manager_id = new SelectList(db.manager, "manager_id", "password");
            return View();
        }

        // POST: difficulty_level_log/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "manager_id,difficulty_id,difficulty_desc,status,datetime")] difficulty_level_log difficulty_level_log)
        {
            if (ModelState.IsValid)
            {
                db.difficulty_level_log.Add(difficulty_level_log);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.difficulty_id = new SelectList(db.difficulty_level, "difficulty_id", "difficulty_desc", difficulty_level_log.difficulty_id);
            ViewBag.manager_id = new SelectList(db.manager, "manager_id", "password", difficulty_level_log.manager_id);
            return View(difficulty_level_log);
        }

        // GET: difficulty_level_log/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            difficulty_level_log difficulty_level_log = db.difficulty_level_log.Find(id);
            if (difficulty_level_log == null)
            {
                return HttpNotFound();
            }
            ViewBag.difficulty_id = new SelectList(db.difficulty_level, "difficulty_id", "difficulty_desc", difficulty_level_log.difficulty_id);
            ViewBag.manager_id = new SelectList(db.manager, "manager_id", "password", difficulty_level_log.manager_id);
            return View(difficulty_level_log);
        }

        // POST: difficulty_level_log/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "manager_id,difficulty_id,difficulty_desc,status,datetime")] difficulty_level_log difficulty_level_log)
        {
            if (ModelState.IsValid)
            {
                db.Entry(difficulty_level_log).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.difficulty_id = new SelectList(db.difficulty_level, "difficulty_id", "difficulty_desc", difficulty_level_log.difficulty_id);
            ViewBag.manager_id = new SelectList(db.manager, "manager_id", "password", difficulty_level_log.manager_id);
            return View(difficulty_level_log);
        }

        // GET: difficulty_level_log/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            difficulty_level_log difficulty_level_log = db.difficulty_level_log.Find(id);
            if (difficulty_level_log == null)
            {
                return HttpNotFound();
            }
            return View(difficulty_level_log);
        }

        // POST: difficulty_level_log/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            difficulty_level_log difficulty_level_log = db.difficulty_level_log.Find(id);
            db.difficulty_level_log.Remove(difficulty_level_log);
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
