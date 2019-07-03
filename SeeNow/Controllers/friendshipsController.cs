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
    public class friendshipsController : Controller
    {
        private SeeNowEntities db = new SeeNowEntities();

        // GET: friendships
        public ActionResult Index()
        {
            var friendship = db.friendship.Include(f => f.users);
            return View(friendship.ToList());
        }

        // GET: friendships/Details/5
        public ActionResult Details(string account)
        {
            if (account == null )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            friendship friendship = db.friendship.Find(account);
            if (friendship == null)
            {
                return HttpNotFound();
            }
            return View(friendship);
        }

        // GET: friendships/Create
        public ActionResult Create()
        {
            ViewBag.account = new SelectList(db.users, "account", "role_id");
            return View();
        }

        // POST: friendships/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "friend_id,account,active")] friendship friendship)
        {
            if (ModelState.IsValid)
            {
                db.friendship.Add(friendship);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.account = new SelectList(db.users, "account", "role_id", friendship.account);
            return View(friendship);
        }

        // GET: friendships/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            friendship friendship = db.friendship.Find(id);
            if (friendship == null)
            {
                return HttpNotFound();
            }
            ViewBag.account = new SelectList(db.users, "account", "role_id", friendship.account);
            return View(friendship);
        }

        // POST: friendships/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "friend_id,account,active")] friendship friendship)
        {
            if (ModelState.IsValid)
            {
                db.Entry(friendship).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.account = new SelectList(db.users, "account", "role_id", friendship.account);
            return View(friendship);
        }

        // GET: friendships/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            friendship friendship = db.friendship.Find(id);
            if (friendship == null)
            {
                return HttpNotFound();
            }
            return View(friendship);
        }

        // POST: friendships/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            friendship friendship = db.friendship.Find(id);
            db.friendship.Remove(friendship);
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
