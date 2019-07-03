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
    public class role_logController : Controller
    {
        private SeeNowEntities db = new SeeNowEntities();

        // GET: role_log
        public ActionResult Index()
        {
            var role_log = db.role_log.Include(r => r.manager).Include(r => r.role);
            return View(role_log.ToList());
        }

        // GET: role_log/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            role_log role_log = db.role_log.Find(id);
            if (role_log == null)
            {
                return HttpNotFound();
            }
            return View(role_log);
        }

        // GET: role_log/Create
        public ActionResult Create()
        {
            ViewBag.manager_id = new SelectList(db.manager, "manager_id", "password");
            ViewBag.role_id = new SelectList(db.role, "role_id", "role_desc");
            return View();
        }

        // POST: role_log/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "manager_id,role_id,role_desc,status,datetime")] role_log role_log)
        {
            if (ModelState.IsValid)
            {
                db.role_log.Add(role_log);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.manager_id = new SelectList(db.manager, "manager_id", "password", role_log.manager_id);
            ViewBag.role_id = new SelectList(db.role, "role_id", "role_desc", role_log.role_id);
            return View(role_log);
        }

        // GET: role_log/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            role_log role_log = db.role_log.Find(id);
            if (role_log == null)
            {
                return HttpNotFound();
            }
            ViewBag.manager_id = new SelectList(db.manager, "manager_id", "password", role_log.manager_id);
            ViewBag.role_id = new SelectList(db.role, "role_id", "role_desc", role_log.role_id);
            return View(role_log);
        }

        // POST: role_log/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "manager_id,role_id,role_desc,status,datetime")] role_log role_log)
        {
            if (ModelState.IsValid)
            {
                db.Entry(role_log).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.manager_id = new SelectList(db.manager, "manager_id", "password", role_log.manager_id);
            ViewBag.role_id = new SelectList(db.role, "role_id", "role_desc", role_log.role_id);
            return View(role_log);
        }

        // GET: role_log/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            role_log role_log = db.role_log.Find(id);
            if (role_log == null)
            {
                return HttpNotFound();
            }
            return View(role_log);
        }

        // POST: role_log/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            role_log role_log = db.role_log.Find(id);
            db.role_log.Remove(role_log);
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
