using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using SeeNow.Models;
using SeeNow.ViewModels;

namespace SeeNow.Controllers
{
    public class setting_menuController : Controller
    {
        private SeeNowEntities db = new SeeNowEntities();

       
        public ActionResult menuPartial()
        {

            //var result = from p in db.setting_menu
            //             join c in db.setting_menu on p.id equals c.parent_id
            //             orderby p.id, c.order_num
            //             select new
            //             {
            //                 Id = p.id,
            //                 TopMenu = p.menu_name_tw,
            //                 TopParentId = p.parent_id,
            //                 SecondMenu = c.menu_name_tw,
            //                 SecondParentId = c.parent_id,
            //                 View = c.view,
            //                 Page = c.page,
            //                 OrderNum = c.order_num
            //             };

            var data = from p in db.setting_menu
                      
                       select new { p.id ,p.menu_name_tw,p.order_num,p.page,p.parent_id,p.view};

            //ViewBag.settingmenu = JsonConvert.SerializeObject(result);
            //DataTable dt = new DataTable();
            DataTable dt = LINQToDataTable(data);

            var firstLevel = dt.Select("parent_id=0");

            var result = new {
                Info = new
                {
                    Level1=from a in firstLevel
                           join b in dt.AsEnumerable() on a.Field<int>("id") equals b.Field<int?>("parent_id") into secondLevel
                           select new
                           {
                               id=a.Field<int>("Id"),
                               title = a.Field<string>("menu_name_tw"),
                               order_num = a.Field<int>("order_num"),
                               Level2= secondLevel.Select(c=> new
                               {
                                   id =c.Field<int>("Id"),
                                   title = c.Field<string>("menu_name_tw"),
                                   page = c.Field<string>("page"),
                                   view = c.Field<string>("view"),
                                   order_num = a.Field<int>("order_num")
                               })
                           }
                }
            };

            string menu= JsonConvert.SerializeObject(result,Formatting.None);

            //HttpResponseMessage message = new HttpResponseMessage { Content = new StringContent(menu, Encoding.GetEncoding("UTF-8"), "application/json") };//这里是去掉反斜杠再放回出去，json就只剩下双引号。
            
            //ViewBag.menu = menu;

            return View(menu);
        }

        public DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();


            // column names
            PropertyInfo[] oProps = null;


            if (varlist == null) return dtReturn;


            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others will follow
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;


                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }


                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }


                DataRow dr = dtReturn.NewRow();


                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }


                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }


        // GET: setting_menu
        public ActionResult Index()
        {
            return View(db.setting_menu.ToList());
        }

        // GET: setting_menu/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            setting_menu setting_menu = db.setting_menu.Find(id);
            if (setting_menu == null)
            {
                return HttpNotFound();
            }
            return View(setting_menu);
        }

        // GET: setting_menu/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: setting_menu/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,menu_name_tw,view,page,parent_id,order_num")] setting_menu setting_menu)
        {
            if (ModelState.IsValid)
            {
                db.setting_menu.Add(setting_menu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(setting_menu);
        }

        // GET: setting_menu/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            setting_menu setting_menu = db.setting_menu.Find(id);
            if (setting_menu == null)
            {
                return HttpNotFound();
            }
            return View(setting_menu);
        }

        // POST: setting_menu/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,menu_name_tw,view,page,parent_id,order_num")] setting_menu setting_menu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(setting_menu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(setting_menu);
        }

        // GET: setting_menu/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            setting_menu setting_menu = db.setting_menu.Find(id);
            if (setting_menu == null)
            {
                return HttpNotFound();
            }
            return View(setting_menu);
        }

        // POST: setting_menu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            setting_menu setting_menu = db.setting_menu.Find(id);
            db.setting_menu.Remove(setting_menu);
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
