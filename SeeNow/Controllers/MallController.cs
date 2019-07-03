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
    public class MallController : Controller
    {
        private SeeNowEntities db = new SeeNowEntities();

        // GET: Mall
        public ActionResult Index()
        {
            return View(db.mall.ToList());
        }

        // GET: Mall/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mall mall = db.mall.Find(id);
            if (mall == null)
            {
                return HttpNotFound();
            }
            return View(mall);
        }

        // GET: Mall/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Mall/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "product_id,product_desc,price,active,img_path,used_img_path")] mall mall)
        {
            if (ModelState.IsValid)
            {
                db.mall.Add(mall);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mall);
        }

        // GET: Mall/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mall mall = db.mall.Find(id);
            if (mall == null)
            {
                return HttpNotFound();
            }
            return View(mall);
        }

        // POST: Mall/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "product_id,product_desc,price,active,img_path,used_img_path")] mall mall)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mall).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mall);
        }

        // GET: Mall/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mall mall = db.mall.Find(id);
            if (mall == null)
            {
                return HttpNotFound();
            }
            return View(mall);
        }

        // POST: Mall/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            mall mall = db.mall.Find(id);
            db.mall.Remove(mall);
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
