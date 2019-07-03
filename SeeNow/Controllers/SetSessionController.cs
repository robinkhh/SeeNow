using SeeNow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeeNow.Controllers
{
    public class SetSessionController : Controller
    {


        public void SetVariable(string key, string value)
        {
            HomeController hc = new HomeController();
            //Session[key] = value;
            System.Web.HttpContext.Current.Session[key] = value;
           
            //return PartialView("_topNavPartial", hc.NavMenu(value_int));
            //return PartialView("_topNavPartial", nav);
            //return this.Json(new { success = true });
        }
    }
}