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
    public class category_logController : Controller
    {
        private SeeNowEntities db = new SeeNowEntities();

        // GET: category_log
        public ActionResult Index()
        {
            var category_log = db.category_log.Include(c => c.category).Include(c => c.manager);
            return View(category_log.ToList());
        }

        // GET: category_log/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            category_log category_log = db.category_log.Find(id);
            if (category_log == null)
            {
                return HttpNotFound();
            }
            return View(category_log);
        }

        // GET: category_log/Create
        public ActionResult Create()
        {
            ViewBag.category_id = new SelectList(db.category, "category_id", "category_desc");
            ViewBag.manager_id = new SelectList(db.manager, "manager_id", "password");
            return View();
        }

        // POST: category_log/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "manager_id,category_id,category_desc,status,datetime")] category_log category_log)
        {
            if (ModelState.IsValid)
            {
                db.category_log.Add(category_log);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.category_id = new SelectList(db.category, "category_id", "category_desc", category_log.category_id);
            ViewBag.manager_id = new SelectList(db.manager, "manager_id", "password", category_log.manager_id);
            return View(category_log);
        }

        // GET: category_log/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            category_log category_log = db.category_log.Find(id);
            if (category_log == null)
            {
                return HttpNotFound();
            }
            ViewBag.category_id = new SelectList(db.category, "category_id", "category_desc", category_log.category_id);
            ViewBag.manager_id = new SelectList(db.manager, "manager_id", "password", category_log.manager_id);
            return View(category_log);
        }

        // POST: category_log/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "manager_id,category_id,category_desc,status,datetime")] category_log category_log)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category_log).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.category_id = new SelectList(db.category, "category_id", "category_desc", category_log.category_id);
            ViewBag.manager_id = new SelectList(db.manager, "manager_id", "password", category_log.manager_id);
            return View(category_log);
        }

        // GET: category_log/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            category_log category_log = db.category_log.Find(id);
            if (category_log == null)
            {
                return HttpNotFound();
            }
            return View(category_log);
        }

        // POST: category_log/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            category_log category_log = db.category_log.Find(id);
            db.category_log.Remove(category_log);
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
