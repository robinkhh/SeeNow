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
    public class quiz_groupController : Controller
    {
        private SeeNowEntities db = new SeeNowEntities();

        // GET: quiz_group
        public ActionResult Index()
        {
            var quiz_group = db.quiz_group.Include(q => q.category).Include(q => q.users);
            return View(quiz_group.ToList());
        }

        // GET: quiz_group/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            quiz_group quiz_group = db.quiz_group.Find(id);
            if (quiz_group == null)
            {
                return HttpNotFound();
            }
            return View(quiz_group);
        }

        // GET: quiz_group/Create
        public ActionResult Create()
        {
            ViewBag.category_id = new SelectList(db.category, "category_id", "category_desc");
            //ViewBag.account = new SelectList(db.users, "account", "role_id");
            ViewBag.account = new SelectList(db.users, "account", "account");

            return View();
        }

        // POST: quiz_group/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "quiz_group1,group_name,account,category_id")] quiz_group quiz_group)
        {
            if (ModelState.IsValid)
            {
                db.quiz_group.Add(quiz_group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.category_id = new SelectList(db.category, "category_id", "category_desc", quiz_group.category_id);
            //ViewBag.account = new SelectList(db.users, "account", "role_id", quiz_group.account);
            ViewBag.account = new SelectList(db.users, "account", "account", quiz_group.account);
            return View(quiz_group);
        }

        // GET: quiz_group/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            quiz_group quiz_group = db.quiz_group.Find(id);
            if (quiz_group == null)
            {
                return HttpNotFound();
            }
            ViewBag.category_id = new SelectList(db.category, "category_id", "category_desc", quiz_group.category_id);
            //ViewBag.account = new SelectList(db.users, "account", "role_id", quiz_group.account);
            ViewBag.account = new SelectList(db.users, "account", "account", quiz_group.account);

            return View(quiz_group);
        }

        // POST: quiz_group/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "quiz_group1,group_name,account,category_id")] quiz_group quiz_group)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quiz_group).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            ViewBag.category_id = new SelectList(db.category, "category_id", "category_desc", quiz_group.category_id);
            ViewBag.account = new SelectList(db.users, "account", "role_id", quiz_group.account);
            return View(quiz_group);
        }

        // GET: quiz_group/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            quiz_group quiz_group = db.quiz_group.Find(id);
            if (quiz_group == null)
            {
                return HttpNotFound();
            }
            return View(quiz_group);
        }

        // POST: quiz_group/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            quiz_group quiz_group = db.quiz_group.Find(id);
            db.quiz_group.Remove(quiz_group);
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
