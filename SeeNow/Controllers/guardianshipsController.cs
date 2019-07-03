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
    public class guardianshipsController : Controller
    {
        private SeeNowEntities db = new SeeNowEntities();

        // GET: guardianships
        public ActionResult Index()
        {
            var guardianship = db.guardianship.Include(g => g.users);
            return View(guardianship.ToList());
        }

        // GET: guardianships/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            guardianship guardianship = db.guardianship.Find(id);
            if (guardianship == null)
            {
                return HttpNotFound();
            }
            return View(guardianship);
        }

        // GET: guardianships/Create
        public ActionResult Create()
        {
            ViewBag.account = new SelectList(db.users, "account", "role_id");
            return View();
        }

        // POST: guardianships/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "parent_id,account")] guardianship guardianship)
        {
            if (ModelState.IsValid)
            {
                db.guardianship.Add(guardianship);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.account = new SelectList(db.users, "account", "role_id", guardianship.account);
            return View(guardianship);
        }

        // GET: guardianships/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            guardianship guardianship = db.guardianship.Find(id);
            if (guardianship == null)
            {
                return HttpNotFound();
            }
            ViewBag.account = new SelectList(db.users, "account", "role_id", guardianship.account);
            return View(guardianship);
        }

        // POST: guardianships/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "parent_id,account")] guardianship guardianship)
        {
            if (ModelState.IsValid)
            {
                db.Entry(guardianship).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.account = new SelectList(db.users, "account", "role_id", guardianship.account);
            return View(guardianship);
        }

        // GET: guardianships/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            guardianship guardianship = db.guardianship.Find(id);
            if (guardianship == null)
            {
                return HttpNotFound();
            }
            return View(guardianship);
        }

        // POST: guardianships/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            guardianship guardianship = db.guardianship.Find(id);
            db.guardianship.Remove(guardianship);
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
