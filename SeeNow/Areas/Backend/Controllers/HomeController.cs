using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SeeNow.Models;

namespace SeeNow.Areas.Backend.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        SeeNowEntities db = new SeeNowEntities();
        
        /// <summary>
        /// 呈現後台使用者登入頁
        /// </summary>
        /// <param name="ReturnUrl">使用者原本Request的Url</param>
        /// <returns></returns>
        [AllowAnonymous]
        #region Login
        public ActionResult Login()//string ReturnUrl
        {
            //ReturnUrl字串是使用者在未登入情況下要求的的Url
            //manager vm = new manager() {  }; //ReturnUrl = ReturnUrl
            ////return PartialView(vm);
            //return View(vm);
            return View();
        }
        #endregion

        [AllowAnonymous]
        [HttpPost]
        #region Login
        public ActionResult Login(string Account, string PD, string ReturnUrl)
        {
            //檢查必填欄位
            if (string.IsNullOrEmpty(Account) || string.IsNullOrEmpty(PD))
            {
                ModelState.AddModelError("manager_id", "請輸入必填欄位");
                ModelState.AddModelError("password", "請輸入必填欄位");
                return View();
            }
            //把輸入的密碼加密
            //string base64PD = Convert.ToBase64String(Encoding.UTF8.GetBytes(PD));

            //todo 和DB裡的資料做比對
            var manager = db.manager.Where(m => m.manager_id == Account).FirstOrDefault();
            if (manager != null)
            {
                string pwd = manager.password;

                if (pwd == PD)
                {
                    //登入成功 
                    //進行表單登入 ※之後使用User.Identity.Name的值就是vm.Account帳號的值
                    FormsAuthentication.SetAuthCookie(Account, true);
                    //進行表單登入  之後User.Identity.Name的值就是Account帳號的值

                    //第二個參數如果是true則cookie留存30分鐘；false則視窗關閉自動失效
                    //並根據web.config的設定自動跳轉道登入後頁面
                    FormsAuthentication.RedirectFromLoginPage(Account, false);

                    //↓這行不會執行到，亂回傳XD
                    //return Content("Login success");
                    Session["account"] = manager.manager_id;
                    return RedirectToAction("Index");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(Account, false);
                    return Content("Login fail");
                }
            }
            else
            {
                FormsAuthentication.SetAuthCookie(Account, false);
                return Content("Login fail");
            }
        }
        #endregion

        /// <summary>
        /// 登入後預設進入的畫面
        /// </summary>
        /// <returns></returns>
        #region Index
        public ActionResult Index()
        {
            //登入帳號
            ViewData["Login_Account"] = User.Identity.Name;
            //是否登入(boolean值)
            ViewData["isLogin"] = User.Identity.IsAuthenticated;

            return View();
        }
        #endregion

        /// <summary>
        /// 後台使用者登出動作
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [OutputCache(Duration = 1)]
        #region Logout
        public ActionResult Logout()
        {
            //清除Session中的資料
            Session.Abandon();
            //登出表單驗證
            FormsAuthentication.SignOut();
            //導至登入頁
            return RedirectToAction("Login", "Home");
        }
        #endregion
    }
}