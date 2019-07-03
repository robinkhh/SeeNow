using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeeNow.Models;

namespace SeeNow.Controllers
{
    public class FrontNavigationController : Controller
    {
        private SeeNowEntities db = new SeeNowEntities();

        //#region TopNav
        //// GET: Navbar
        //public ActionResult TopNav(int role_type=0)
        //{
         
        //    List<frontend_menu> nav = new List<frontend_menu>();

        //    switch(role_type)
        //    { 
        //        case 2:
        //            nav = db.frontend_menu.OrderBy(f => f.order_num).Where(f => f.student_auth == true).ToList();
        //            break;
        //        case 3:
        //            nav = db.frontend_menu.OrderBy(f => f.order_num).Where(f => f.teacher_auth == true).ToList();
        //            break;
        //        case 4:
        //            nav = db.frontend_menu.OrderBy(f => f.order_num).Where(f => f.parent_auth == true).ToList();
        //            break;
        //        default:
        //            nav = db.frontend_menu.OrderBy(f => f.order_num).Where(f => f.guest_auth == true).ToList();
        //            break;
        //    }

        //    return PartialView("_topNav", nav);
        //}
        //#endregion
    }
}