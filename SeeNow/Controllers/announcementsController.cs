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
    public class announcementsController : Controller
    {
        private SeeNowEntities db = new SeeNowEntities();

        public ActionResult Index()
        {
            return View(db.announcement.ToList());
        }

        public ActionResult _AnnouncementIndex(int number = 0)
        {
            IList<announcement> announcements;
            if (number == 0)
            {
                announcements = db.announcement.OrderBy(p => p.priority).ToList();//用priority順排再用publish_time逆排
            }
            else
            {
                announcements = db.announcement.OrderBy(p => p.priority).Take(number).ToList();
            }
            return PartialView("_AnnouncementIndex", announcements);
        }

        // GET: announcements/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            announcement announcement = db.announcement.Find(id);
            if (announcement == null)
            {
                return HttpNotFound();
            }
            return View(announcement);
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
