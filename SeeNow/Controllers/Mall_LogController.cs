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
    public class Mall_LogController : Controller
    {
        private SeeNowEntities db = new SeeNowEntities();

        // GET: Mall_Log
        public ActionResult Index()
        {
            var mall_log = db.mall_log.Include(m => m.mall).Include(m => m.manager);
            return View(mall_log.ToList());
        }

        // GET: Mall_Log/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mall_log mall_log = db.mall_log.Find(id);
            if (mall_log == null)
            {
                return HttpNotFound();
            }
            return View(mall_log);
        }

        // GET: Mall_Log/Create
        public ActionResult Create()
        {
            ViewBag.product_id = new SelectList(db.mall, "product_id", "product_desc");
            ViewBag.manager_id = new SelectList(db.manager, "manager_id", "password");
            return View();
        }

        // POST: Mall_Log/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "manager_id,product_id,product_desc,price,active,img_path,used_img_path,status,datetime")] mall_log mall_log)
        {
            if (ModelState.IsValid)
            {
                db.mall_log.Add(mall_log);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.product_id = new SelectList(db.mall, "product_id", "product_desc", mall_log.product_id);
            ViewBag.manager_id = new SelectList(db.manager, "manager_id", "password", mall_log.manager_id);
            return View(mall_log);
        }

        // GET: Mall_Log/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mall_log mall_log = db.mall_log.Find(id);
            if (mall_log == null)
            {
                return HttpNotFound();
            }
            ViewBag.product_id = new SelectList(db.mall, "product_id", "product_desc", mall_log.product_id);
            ViewBag.manager_id = new SelectList(db.manager, "manager_id", "password", mall_log.manager_id);
            return View(mall_log);
        }

        // POST: Mall_Log/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "manager_id,product_id,product_desc,price,active,img_path,used_img_path,status,datetime")] mall_log mall_log)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mall_log).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.product_id = new SelectList(db.mall, "product_id", "product_desc", mall_log.product_id);
            ViewBag.manager_id = new SelectList(db.manager, "manager_id", "password", mall_log.manager_id);
            return View(mall_log);
        }

        // GET: Mall_Log/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mall_log mall_log = db.mall_log.Find(id);
            if (mall_log == null)
            {
                return HttpNotFound();
            }
            return View(mall_log);
        }

        // POST: Mall_Log/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            mall_log mall_log = db.mall_log.Find(id);
            db.mall_log.Remove(mall_log);
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
