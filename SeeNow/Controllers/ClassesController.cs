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
    public class ClassesController : Controller
    {
        private SeeNowEntities db = new SeeNowEntities();

        // GET: Classes
        public ActionResult Index()
        {
            var classes = db.classes.Include(c => c.users);
            return View(classes.ToList());
        }

        // GET: Classes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            classes classes = db.classes.Find(id);
            if (classes == null)
            {
                return HttpNotFound();
            }
            return View(classes);
        }

        // GET: Classes/Create
        public ActionResult Create()
        {
            ViewBag.teacher_id = new SelectList(db.users, "account", "role_id");
            return View();
        }

        // POST: Classes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "class_id,teacher_id,class_name,visible,datetime,class_code")] classes classes)
        {
            if (ModelState.IsValid)
            {
                db.classes.Add(classes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.teacher_id = new SelectList(db.users, "account", "role_id", classes.teacher_id);
            return View(classes);
        }

        // GET: Classes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            classes classes = db.classes.Find(id);
            if (classes == null)
            {
                return HttpNotFound();
            }
            ViewBag.teacher_id = new SelectList(db.users, "account", "role_id", classes.teacher_id);
            return View(classes);
        }

        // POST: Classes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "class_id,teacher_id,class_name,visible,datetime,class_code")] classes classes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(classes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.teacher_id = new SelectList(db.users, "account", "role_id", classes.teacher_id);
            return View(classes);
        }

        // GET: Classes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            classes classes = db.classes.Find(id);
            if (classes == null)
            {
                return HttpNotFound();
            }
            return View(classes);
        }

        // POST: Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            classes classes = db.classes.Find(id);
            db.classes.Remove(classes);
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
