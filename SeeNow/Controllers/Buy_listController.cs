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
    public class Buy_listController : Controller
    {
        private SeeNowEntities db = new SeeNowEntities();

        // GET: Buy_list
        public ActionResult Index()
        {
            var buy_list = db.buy_list.Include(b => b.users).Include(b => b.mall);
            return View(buy_list.ToList());
        }

        // GET: Buy_list/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            buy_list buy_list = db.buy_list.Find(id);
            if (buy_list == null)
            {
                return HttpNotFound();
            }
            return View(buy_list);
        }

        // GET: Buy_list/Create
        public ActionResult Create()
        {
            ViewBag.account = new SelectList(db.users, "account", "role_id");
            ViewBag.product_id = new SelectList(db.mall, "product_id", "product_desc");
            return View();
        }

        // POST: Buy_list/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "account,product_id,quid,number,datetime")] buy_list buy_list)
        {
            if (ModelState.IsValid)
            {
                db.buy_list.Add(buy_list);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.account = new SelectList(db.users, "account", "role_id", buy_list.account);
            ViewBag.product_id = new SelectList(db.mall, "product_id", "product_desc", buy_list.product_id);
            return View(buy_list);
        }

        // GET: Buy_list/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            buy_list buy_list = db.buy_list.Find(id);
            if (buy_list == null)
            {
                return HttpNotFound();
            }
            ViewBag.account = new SelectList(db.users, "account", "role_id", buy_list.account);
            ViewBag.product_id = new SelectList(db.mall, "product_id", "product_desc", buy_list.product_id);
            return View(buy_list);
        }

        // POST: Buy_list/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "account,product_id,quid,number,datetime")] buy_list buy_list)
        {
            if (ModelState.IsValid)
            {
                db.Entry(buy_list).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.account = new SelectList(db.users, "account", "role_id", buy_list.account);
            ViewBag.product_id = new SelectList(db.mall, "product_id", "product_desc", buy_list.product_id);
            return View(buy_list);
        }

        // GET: Buy_list/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            buy_list buy_list = db.buy_list.Find(id);
            if (buy_list == null)
            {
                return HttpNotFound();
            }
            return View(buy_list);
        }

        // POST: Buy_list/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            buy_list buy_list = db.buy_list.Find(id);
            db.buy_list.Remove(buy_list);
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
