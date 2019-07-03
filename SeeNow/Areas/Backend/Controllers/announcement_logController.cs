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
    public class announcement_logController : Controller
    {
        private SeeNowEntities db = new SeeNowEntities();

        // GET: announcement_log
        public ActionResult Index()
        {
            var announcement_log = db.announcement_log.Include(a => a.announcement).Include(a => a.manager);
            return View(announcement_log.ToList());
        }

        // GET: announcement_log/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            announcement_log announcement_log = db.announcement_log.Find(id);
            if (announcement_log == null)
            {
                return HttpNotFound();
            }
            return View(announcement_log);
        }

        // GET: announcement_log/Create
        public ActionResult Create()
        {
            ViewBag.id = new SelectList(db.announcement, "id", "title");
            ViewBag.manager_id = new SelectList(db.manager, "manager_id", "password");
            return View();
        }

        // POST: announcement_log/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "manager_id,id,title,content,publish_time,priority,status,datetime")] announcement_log announcement_log)
        {
            if (ModelState.IsValid)
            {
                db.announcement_log.Add(announcement_log);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id = new SelectList(db.announcement, "id", "title", announcement_log.id);
            ViewBag.manager_id = new SelectList(db.manager, "manager_id", "password", announcement_log.manager_id);
            return View(announcement_log);
        }

        // GET: announcement_log/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            announcement_log announcement_log = db.announcement_log.Find(id);
            if (announcement_log == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = new SelectList(db.announcement, "id", "title", announcement_log.id);
            ViewBag.manager_id = new SelectList(db.manager, "manager_id", "password", announcement_log.manager_id);
            return View(announcement_log);
        }

        // POST: announcement_log/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "manager_id,id,title,content,publish_time,priority,status,datetime")] announcement_log announcement_log)
        {
            if (ModelState.IsValid)
            {
                db.Entry(announcement_log).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id = new SelectList(db.announcement, "id", "title", announcement_log.id);
            ViewBag.manager_id = new SelectList(db.manager, "manager_id", "password", announcement_log.manager_id);
            return View(announcement_log);
        }

        // GET: announcement_log/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            announcement_log announcement_log = db.announcement_log.Find(id);
            if (announcement_log == null)
            {
                return HttpNotFound();
            }
            return View(announcement_log);
        }

        // POST: announcement_log/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            announcement_log announcement_log = db.announcement_log.Find(id);
            db.announcement_log.Remove(announcement_log);
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
