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
    public class Profile_LogController : Controller
    {
        private SeeNowEntities db = new SeeNowEntities();

        // GET: Profile_Log
        public ActionResult Index()
        {
            var profile_log = db.profile_log.Include(p => p.manager).Include(p => p.profile);
            return View(profile_log.ToList());
        }

        // GET: Profile_Log/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            profile_log profile_log = db.profile_log.Find(id);
            if (profile_log == null)
            {
                return HttpNotFound();
            }
            return View(profile_log);
        }

        // GET: Profile_Log/Create
        public ActionResult Create()
        {
            ViewBag.manager_id = new SelectList(db.manager, "manager_id", "password");
            ViewBag.profile_id = new SelectList(db.profile, "profile_id", "profile_name");
            return View();
        }

        // POST: Profile_Log/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "manager_id,profile_id,profile_name,profile_path,status,datetime")] profile_log profile_log)
        {
            if (ModelState.IsValid)
            {
                db.profile_log.Add(profile_log);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.manager_id = new SelectList(db.manager, "manager_id", "password", profile_log.manager_id);
            ViewBag.profile_id = new SelectList(db.profile, "profile_id", "profile_name", profile_log.profile_id);
            return View(profile_log);
        }

        // GET: Profile_Log/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            profile_log profile_log = db.profile_log.Find(id);
            if (profile_log == null)
            {
                return HttpNotFound();
            }
            ViewBag.manager_id = new SelectList(db.manager, "manager_id", "password", profile_log.manager_id);
            ViewBag.profile_id = new SelectList(db.profile, "profile_id", "profile_name", profile_log.profile_id);
            return View(profile_log);
        }

        // POST: Profile_Log/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "manager_id,profile_id,profile_name,profile_path,status,datetime")] profile_log profile_log)
        {
            if (ModelState.IsValid)
            {
                db.Entry(profile_log).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.manager_id = new SelectList(db.manager, "manager_id", "password", profile_log.manager_id);
            ViewBag.profile_id = new SelectList(db.profile, "profile_id", "profile_name", profile_log.profile_id);
            return View(profile_log);
        }

        // GET: Profile_Log/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            profile_log profile_log = db.profile_log.Find(id);
            if (profile_log == null)
            {
                return HttpNotFound();
            }
            return View(profile_log);
        }

        // POST: Profile_Log/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            profile_log profile_log = db.profile_log.Find(id);
            db.profile_log.Remove(profile_log);
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
