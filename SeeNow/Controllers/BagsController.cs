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
    public class BagsController : Controller
    {
        private SeeNowEntities db = new SeeNowEntities();

        // GET: Bags
        public ActionResult Index()
        {
            var bag = db.bag.Include(b => b.users).Include(b => b.mall);
            return View(bag.ToList());
        }

        // GET: Bags/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bag bag = db.bag.Find(id);
            if (bag == null)
            {
                return HttpNotFound();
            }
            return View(bag);
        }

        // GET: Bags/Create
        public ActionResult Create()
        {
            ViewBag.account = new SelectList(db.users, "account", "role_id");
            ViewBag.product_id = new SelectList(db.mall, "product_id", "product_desc");
            return View();
        }

        // POST: Bags/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "account,product_id,remaining_quantity")] bag bag)
        {
            if (ModelState.IsValid)
            {
                db.bag.Add(bag);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.account = new SelectList(db.users, "account", "role_id", bag.account);
            ViewBag.product_id = new SelectList(db.mall, "product_id", "product_desc", bag.product_id);
            return View(bag);
        }

        // GET: Bags/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bag bag = db.bag.Find(id);
            if (bag == null)
            {
                return HttpNotFound();
            }
            ViewBag.account = new SelectList(db.users, "account", "role_id", bag.account);
            ViewBag.product_id = new SelectList(db.mall, "product_id", "product_desc", bag.product_id);
            return View(bag);
        }

        // POST: Bags/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "account,product_id,remaining_quantity")] bag bag)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bag).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.account = new SelectList(db.users, "account", "role_id", bag.account);
            ViewBag.product_id = new SelectList(db.mall, "product_id", "product_desc", bag.product_id);
            return View(bag);
        }

        // GET: Bags/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bag bag = db.bag.Find(id);
            if (bag == null)
            {
                return HttpNotFound();
            }
            return View(bag);
        }

        // POST: Bags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            bag bag = db.bag.Find(id);
            db.bag.Remove(bag);
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
