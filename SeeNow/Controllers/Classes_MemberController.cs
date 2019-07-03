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
    public class Classes_MemberController : Controller
    {
        private SeeNowEntities db = new SeeNowEntities();

        // GET: Classes_Member
        public ActionResult Index()
        {
            var classes_member = db.classes_member.Include(c => c.classes).Include(c => c.users);
            return View(classes_member.ToList());
        }

        // GET: Classes_Member/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            classes_member classes_member = db.classes_member.Find(id);
            if (classes_member == null)
            {
                return HttpNotFound();
            }
            return View(classes_member);
        }

        // GET: Classes_Member/Create
        public ActionResult Create()
        {
            ViewBag.class_id = new SelectList(db.classes, "class_id", "teacher_id");
            ViewBag.student_id = new SelectList(db.users, "account", "role_id");
            return View();
        }

        // POST: Classes_Member/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "student_id,class_id,datetime")] classes_member classes_member)
        {
            if (ModelState.IsValid)
            {
                db.classes_member.Add(classes_member);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.class_id = new SelectList(db.classes, "class_id", "teacher_id", classes_member.class_id);
            ViewBag.student_id = new SelectList(db.users, "account", "role_id", classes_member.student_id);
            return View(classes_member);
        }

        // GET: Classes_Member/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            classes_member classes_member = db.classes_member.Find(id);
            if (classes_member == null)
            {
                return HttpNotFound();
            }
            ViewBag.class_id = new SelectList(db.classes, "class_id", "teacher_id", classes_member.class_id);
            ViewBag.student_id = new SelectList(db.users, "account", "role_id", classes_member.student_id);
            return View(classes_member);
        }

        // POST: Classes_Member/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "student_id,class_id,datetime")] classes_member classes_member)
        {
            if (ModelState.IsValid)
            {
                db.Entry(classes_member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.class_id = new SelectList(db.classes, "class_id", "teacher_id", classes_member.class_id);
            ViewBag.student_id = new SelectList(db.users, "account", "role_id", classes_member.student_id);
            return View(classes_member);
        }

        // GET: Classes_Member/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            classes_member classes_member = db.classes_member.Find(id);
            if (classes_member == null)
            {
                return HttpNotFound();
            }
            return View(classes_member);
        }

        // POST: Classes_Member/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            classes_member classes_member = db.classes_member.Find(id);
            db.classes_member.Remove(classes_member);
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
