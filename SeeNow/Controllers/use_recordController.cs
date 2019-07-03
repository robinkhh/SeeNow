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
    public class use_recordController : Controller
    {
        private SeeNowEntities db = new SeeNowEntities();

        // GET: use_record
        public ActionResult Index()
        {
            var use_record = db.use_record.Include(u => u.mall).Include(u => u.message).Include(u => u.quizzes).Include(u => u.users);
            return View(use_record.ToList());
        }

        // GET: use_record/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            use_record use_record = db.use_record.Find(id);
            if (use_record == null)
            {
                return HttpNotFound();
            }
            return View(use_record);
        }

        // GET: use_record/Create
        public ActionResult Create()
        {
            ViewBag.product_id = new SelectList(db.mall, "product_id", "product_desc");
            ViewBag.message_guid = new SelectList(db.message, "message_guid", "from_id");
            ViewBag.quiz_guid = new SelectList(db.quizzes, "quiz_guid", "type_id");
            ViewBag.account = new SelectList(db.users, "account", "role_id");
            return View();
        }

        // POST: use_record/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "use_guid,account,product_id,used_quantity,message_guid,quiz_guid")] use_record use_record)
        {
            if (ModelState.IsValid)
            {
                db.use_record.Add(use_record);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.product_id = new SelectList(db.mall, "product_id", "product_desc", use_record.product_id);
            ViewBag.message_guid = new SelectList(db.message, "message_guid", "from_id", use_record.message_guid);
            ViewBag.quiz_guid = new SelectList(db.quizzes, "quiz_guid", "type_id", use_record.quiz_guid);
            ViewBag.account = new SelectList(db.users, "account", "role_id", use_record.account);
            return View(use_record);
        }

        // GET: use_record/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            use_record use_record = db.use_record.Find(id);
            if (use_record == null)
            {
                return HttpNotFound();
            }
            ViewBag.product_id = new SelectList(db.mall, "product_id", "product_desc", use_record.product_id);
            ViewBag.message_guid = new SelectList(db.message, "message_guid", "from_id", use_record.message_guid);
            ViewBag.quiz_guid = new SelectList(db.quizzes, "quiz_guid", "type_id", use_record.quiz_guid);
            ViewBag.account = new SelectList(db.users, "account", "role_id", use_record.account);
            return View(use_record);
        }

        // POST: use_record/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "use_guid,account,product_id,used_quantity,message_guid,quiz_guid")] use_record use_record)
        {
            if (ModelState.IsValid)
            {
                db.Entry(use_record).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.product_id = new SelectList(db.mall, "product_id", "product_desc", use_record.product_id);
            ViewBag.message_guid = new SelectList(db.message, "message_guid", "from_id", use_record.message_guid);
            ViewBag.quiz_guid = new SelectList(db.quizzes, "quiz_guid", "type_id", use_record.quiz_guid);
            ViewBag.account = new SelectList(db.users, "account", "role_id", use_record.account);
            return View(use_record);
        }

        // GET: use_record/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            use_record use_record = db.use_record.Find(id);
            if (use_record == null)
            {
                return HttpNotFound();
            }
            return View(use_record);
        }

        // POST: use_record/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            use_record use_record = db.use_record.Find(id);
            db.use_record.Remove(use_record);
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
