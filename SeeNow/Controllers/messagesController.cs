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
    public class messagesController : Controller
    {
        private SeeNowEntities db = new SeeNowEntities();

        // GET: messages
        public ActionResult Index()
        {
            var message = db.message.Include(m => m.users).Include(m => m.users1);
            return View(message.ToList());
        }

        // GET: messages/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            message message = db.message.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // GET: messages/Create
        public ActionResult Create()
        {
            ViewBag.from_id = new SelectList(db.users, "account", "role_id");
            ViewBag.to_id = new SelectList(db.users, "account", "role_id");
            return View();
        }

        // POST: messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "message_guid,from_id,to_id,msg,datetime,read_flag,delete_flag")] message message)
        {
            if (ModelState.IsValid)
            {
                db.message.Add(message);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.from_id = new SelectList(db.users, "account", "role_id", message.from_id);
            ViewBag.to_id = new SelectList(db.users, "account", "role_id", message.to_id);
            return View(message);
        }

        // GET: messages/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            message message = db.message.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            ViewBag.from_id = new SelectList(db.users, "account", "role_id", message.from_id);
            ViewBag.to_id = new SelectList(db.users, "account", "role_id", message.to_id);
            return View(message);
        }

        // POST: messages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "message_guid,from_id,to_id,msg,datetime,read_flag,delete_flag")] message message)
        {
            if (ModelState.IsValid)
            {
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.from_id = new SelectList(db.users, "account", "role_id", message.from_id);
            ViewBag.to_id = new SelectList(db.users, "account", "role_id", message.to_id);
            return View(message);
        }

        // GET: messages/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            message message = db.message.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            message message = db.message.Find(id);
            db.message.Remove(message);
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
