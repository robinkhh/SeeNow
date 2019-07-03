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
            ViewBag.category = new SelectList(db.category, "category_id", "category_desc");
            return View(db.quiz_group.ToList());
        }

        // POST: quiz_group/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int category, string group_name, string account)
        {
            var gname = db.quiz_group.Where(m => m.group_name == group_name).FirstOrDefault();

            if (gname == null)
            {
                var qgid = db.quiz_group.OrderByDescending(m => m.quiz_group1).FirstOrDefault();

                quiz_group qg = new quiz_group();
                qg.category_id = category;
                qg.quiz_group1 = qgid.quiz_group1 + 1;

                qg.group_name = group_name;
                qg.account = account;

                db.quiz_group.Add(qg);
                db.SaveChanges();

                //return RedirectToAction("QGQAIndex");
                return RedirectToAction("Index", "quiz_group");
            }
            ViewBag.Message = "遊戲題組名稱，已有人使用!!";
            return View(db.quiz_group.ToList());
        }

        // GET: quiz_group/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            quiz_group quiz_group = db.quiz_group.Find(id);
            string nick_name = db.users.Where(u => u.account == quiz_group.account).FirstOrDefault().nick_name;
            if (quiz_group == null)
            {
                return HttpNotFound();
            }
            ViewBag.nick_name = nick_name;
            ViewBag.category = new SelectList(db.category, "category_id", "category_desc", quiz_group.category_id);
            //ViewBag.account = new SelectList(db.users, "account", "role_id", quiz_group.account);
            //ViewBag.account = new SelectList(db.users, "account", "account", quiz_group.account);

            return View(quiz_group);
        }

        // POST: quiz_group/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int quiz_group1,int category, string group_name, string account)
        {
            quiz_group qg = db.quiz_group.Where(q => q.quiz_group1 == quiz_group1).FirstOrDefault();

            qg.group_name = group_name;
            qg.account = Session["user_id"].ToString();
            qg.category_id = category;

            db.SaveChanges();

            return RedirectToAction("Index", "quiz_group");

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
