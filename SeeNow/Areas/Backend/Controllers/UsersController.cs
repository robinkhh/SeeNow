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
    public class UsersController : Controller
    {
        private SeeNowEntities db = new SeeNowEntities();

        // GET: Users
        #region Index
        public ActionResult Index()
        {
            var users = db.users.Include(u => u.profile).Include(u => u.role);
            return View(users.ToList());
        }
        #endregion

        #region Index
        public ActionResult FEdit()
        {

            users users = db.users.Find(Session["user_id"].ToString());

            ViewBag.profile = new SelectList(db.profile, "profile_id", "profile_path",users.profile_id);
            ViewBag.role_id = new SelectList(db.role, "role_id", "role_desc",users.role_id);
            return View(users);
        }
        #endregion

        [HttpPost]
        //[ValidateAntiForgeryToken]
        #region FEdit
        public ActionResult FEdit([Bind(Include = "account,role_id,password,nick_name,e_mail,score,energy,profile_id,bag_number,lock_flag,validation_flag,resetable")] users users)
        {
            if (ModelState.IsValid)
            {
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
                return View(users);
            }
            ViewBag.profile_id = new SelectList(db.profile, "profile_id", "profile_name", users.profile_id);
            ViewBag.role_id = new SelectList(db.role, "role_id", "role_desc", users.role_id);
            return View(users);
        }
        #endregion Edit

        // GET: Users/Details/5
        #region Details
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            users users = db.users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }
        #endregion

        // GET: Users/Create
        #region Create
        public ActionResult Create()
        {
            ViewBag.profile_id = new SelectList(db.profile, "profile_id", "profile_name");
            ViewBag.role_id = new SelectList(db.role, "role_id", "role_desc");
            return View();
        }
        #endregion

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        #region Create
        public ActionResult Create([Bind(Include = "account,role_id,password,nick_name,e_mail,score,energy,profile_id,bag_number,lock_flag,validation_flag,resetable")] users users)
        {
            if (ModelState.IsValid)
            {
                db.users.Add(users);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.profile_id = new SelectList(db.profile, "profile_id", "profile_name", users.profile_id);
            ViewBag.role_id = new SelectList(db.role, "role_id", "role_desc", users.role_id);
            return View(users);
        }
        #endregion

        // GET: Users/Edit/5
        #region Edit
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            users users = db.users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            ViewBag.profile_id = new SelectList(db.profile, "profile_id", "profile_name", users.profile_id);
            ViewBag.role_id = new SelectList(db.role, "role_id", "role_desc", users.role_id);
            return View(users);
        }
        #endregion

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        #region Edit
        public ActionResult Edit([Bind(Include = "account,role_id,password,nick_name,e_mail,score,energy,profile_id,bag_number,lock_flag,validation_flag,resetable")] users users)
        {
            if (ModelState.IsValid)
            {
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.profile_id = new SelectList(db.profile, "profile_id", "profile_name", users.profile_id);
            ViewBag.role_id = new SelectList(db.role, "role_id", "role_desc", users.role_id);
            return View(users);
        }
        #endregion Edit

        // GET: Users/Delete/5
        #region Delete
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            users users = db.users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }
        #endregion

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        #region DeleteConfirmed
        public ActionResult DeleteConfirmed(string id)
        {
            users users = db.users.Find(id);
            db.users.Remove(users);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}
