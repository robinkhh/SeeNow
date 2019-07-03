using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeeNow.Models;
using System.Data.Entity;
using System.Net;
using System.Web.Security;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SeeNow.Controllers
{
    public class HomeController : Controller
    {
        private SeeNowEntities db = new SeeNowEntities();

        public ActionResult Index()
        {
            
            var users = db.users.Include(u => u.profile).Include(u => u.role);
            return View(users.ToList());
        }

        #region Registered 初始註冊頁面
        public ActionResult Registered()
        {
            ViewBag.profile = new SelectList(db.profile, "profile_id", "profile_path");
            ViewBag.role_id = new SelectList(db.role, "role_id", "role_desc");
            return View();
        }
        #endregion

        #region Registered 註冊
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registered(string account,string role_id,string password,string nick_name,string e_mail,short profile_id)
        {
            //檢查帳號或email是否存在，存在則不可重複申請
            var user = db.users.Where(m => m.account == account || m.e_mail== e_mail).FirstOrDefault();
            if (user != null)
            {
                Response.Write("<script>alert('帳號或信箱重複，請修改!！');</script>");
                ViewBag.profile = new SelectList(db.profile, "profile_id", "profile_path");
                ViewBag.role_id = new SelectList(db.role, "role_id", "role_desc");
                return View();
            }
            else
            {

                users newuser = new users();
                newuser.account = account;
                newuser.role_id = role_id;
                newuser.password = password;
                newuser.nick_name = nick_name;
                newuser.e_mail = e_mail;
                newuser.profile_id = profile_id;

                db.users.Add(newuser);
                db.SaveChanges();

                ViewBag.mail = e_mail;
                ViewBag.account = account;

                SendAuthMail(e_mail, account);
                return View("SendAuthMail");
                
            }
            
        }
        #endregion

        #region Login
        public ActionResult Login()
        {
            
            //ViewBag.profile_id = new SelectList(db.profile, "profile_id", "profile_name");
            ViewBag.role_id = new SelectList(db.role, "role_id", "role_desc");
            return View();
        }
        #endregion


        #region Login 登入
        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string account,string role_id,string password,string txtcode)
        {
            

            if (Session["ValiCode"] == null)
            {
                ViewBag.msg="閒置時間過長，請重新輸入!!";
                return View();
            }
            if (Session["ValiCode"].ToString() != txtcode)
            {
                ViewBag.msg="驗證碼錯誤，請重新輸入!!";
                return View();
            }
            else
            {

                //檢查必填欄位
                if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
                {
                    ModelState.AddModelError("accout", "請輸入必填欄位");
                    ModelState.AddModelError("password", "請輸入必填欄位");
                    return View();
                }
                //把輸入的密碼加密
                //string base64PD = Convert.ToBase64String(Encoding.UTF8.GetBytes(PD));

                //todo 和DB裡的資料做比對
                var user = db.users.Where(m => m.account == account).FirstOrDefault();
                if (user != null)
                {
                    string pwd = user.password;
                    if (user.resetable == false)
                    {
                        if (pwd == password)
                        {
                            //登入成功 
                            //進行表單登入 ※之後使用User.Identity.Name的值就是vm.Account帳號的值
                            //FormsAuthentication.SetAuthCookie(account, true);
                            //進行表單登入  之後User.Identity.Name的值就是Account帳號的值

                            //第二個參數如果是true則cookie留存30分鐘；false則視窗關閉自動失效
                            //並根據web.config的設定自動跳轉道登入後頁面
                            //FormsAuthentication.RedirectFromLoginPage(account, false);
                            Session["user_role"] = user.role_id;

                            Session["role_desc"] = GetRole(user.role_id);

                            Session["user_name"] = user.nick_name;
                            Session["user_id"] = user.account;

                            var profile = db.profile.Where(p => p.profile_id == user.profile_id).FirstOrDefault();
                            Session["profile"] = profile.profile_path;
                            //↓這行不會執行到，亂回傳XD
                            return RedirectToAction("PlayerStart", "Game");
                        }
                        else
                        {
                            //FormsAuthentication.SetAuthCookie(account, false);
                            ViewBag.msg = "帳號或密碼錯誤!!";
                            return View();
                            //return Content("Login fail");
                        }
                    }
                    else {
                        
                        ViewBag.msg = "請重新設定密碼!!";
                        return RedirectToAction("ResetPWD","Home",new { account=account});
                    }
                }
                else
                {
                    //FormsAuthentication.SetAuthCookie(account, false);
                    ViewBag.msg = "帳號或密碼錯誤!!";
                    return View();
                    //return Content("Login fail");
                }
            }
        }
        #endregion

        #region ForgetPWD 忘記密碼初始頁面
        public ActionResult ForgetPWD()
        {
            return View();
        }
        #endregion

        #region ForgetPWD 忘記密碼
        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgetPWD(string account, string email, string txtcode)
        {


            if (Session["ValiCode"] == null)
            {
                ViewBag.msg = "閒置時間過長，請重新輸入!!";
                return View();
            }
            if (Session["ValiCode"].ToString() != txtcode)
            {
                ViewBag.msg = "驗證碼錯誤，請重新輸入!!";
                return View();
            }
            else
            {

                //檢查必填欄位
                if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(email))
                {
                    ModelState.AddModelError("accout", "請輸入必填欄位");
                    ModelState.AddModelError("email", "請輸入必填欄位");
                    return View();
                }
                //把輸入的密碼加密
                //string base64PD = Convert.ToBase64String(Encoding.UTF8.GetBytes(PD));
                
                //確認帳號信箱相同
                var user = db.users.Where(m => m.account == account && m.e_mail==email).FirstOrDefault();
                if (user != null)
                {
                    //重設隨機密碼
                    Validation vCode = new Validation();
                    string newPWD = vCode.RandomCode(10);

                    user.resetable = true;
                    user.password = newPWD;
                    db.SaveChanges();
                    //有該使用者則寄送信箱 無頁面

                    MailBox.SendMail(account, email,
               "SeeNow會員忘記密碼信",
               "您的新密碼是:"+newPWD+"<br/>請點擊下列超連結重設密碼<br /> <br /><a href='http://10.10.3.200/" + Url.Action("ResetPWD", "Home") + "?account="+ account + "' >請點我</a>");

                    ViewBag.msg = "寄送成功，請至郵箱收取信件並重設密碼!!";
                    return View();
                   
                }
                else
                {
                    //FormsAuthentication.SetAuthCookie(account, false);
                    ViewBag.msg = "查無該帳號或信箱!!";
                    return View();
                    //return Content("Login fail");
                }
            }
        }
        #endregion

        #region ResetPWD 重設密碼初始頁面
        public ActionResult ResetPWD(string account)
        {
            //var user = db.users.Where(u => u.account == account && u.e_mail==email).FirstOrDefault();
            ViewBag.account = account;
           

            return View();
        }
        #endregion

        #region ResetPWD 重設密碼
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPWD(string account, string oldpassword, string password)
        {
            var user = db.users.Where(u => u.account == account).FirstOrDefault();
            ViewBag.account = account;
            if (user.password == oldpassword  )
            {
                if (user.resetable == true)
                {
                    //密碼輸入正確
                    user.password = password;
                    user.resetable = false;
                    db.SaveChanges();
                    ViewBag.msg = "密碼已重設完成，請重新登入!!";

                    return View();
                }
                else {
                    //密碼已重設完成
                    ViewBag.msg = "密碼已重設完成，不需要重設!!";
                    return View();
                }
            }
            else {
                //密碼輸入錯誤
                ViewBag.msg = "信件新密碼輸入錯誤!!";
                return View();
            }

            //return RedirectToAction("Login", "Home");
        }
        #endregion

        #region Logout 登出
        public ActionResult Logout() {
            Session["user_role"] = null;
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region GetValidateCode 繪製驗證碼圖形
        public ActionResult GetValidateCode()
        {
            Validation vCode = new Validation();
            byte[] bytes;
            string code;
            vCode.GetValidateCode(out code,out bytes);
            Session["ValiCode"] = code;
            return File(bytes, @"image/jpeg");
        }
        #endregion

        #region SendAuthMail 寄認證信 無頁面
        protected void SendAuthMail(string toMail, string account)
        {
            MailBox.SendMail(account, toMail, 
                "SeeNow會員註冊認證信", 
                "請點擊下列超連結完成會員註冊認證<br /> <br /><a href='http://10.10.3.200/" + Url.Action("AuthOK", "Home") + "?account=" + account + "' >請點我</a>");

        }
        #endregion

        #region AuthOK 認證信驗證頁面
        public ActionResult AuthOK(string account)
        {
            var user = db.users.Where(m => m.account == account).FirstOrDefault();
            if (user != null)
            {
                if (user.validation_flag != true)
                {
                    user.validation_flag = true;
                    db.SaveChanges();

                    Response.Write("<header class='masthead'><div class='container'><h2 class='text-center'>恭喜您完成會員認證！</h2></div></header>");
                    return View();
                }
                else
                {
                    Response.Write("<header class='masthead'><div class='container'><h2 class='text-center'>會員已認證過，請不要重複認證!！!</h2></div></header>");
                    return View();
                }
                //return RedirectToAction("Login");
            }
            else
            {
                Response.Write("<header class='masthead'><div class='container'><h2 class='text-center'>查無此會員,請註冊!!</h2></div></header>");
                return View();
                //return RedirectToAction("Registered");
            }

            
        }
        #endregion

        #region TopNav 上方選單控制
        // GET: Navbar
        public ActionResult TopNav(int role_type = 0)
        {
            return PartialView("_topNavPartial", NavMenu(role_type));
        }
        #endregion

        public List<frontend_menu> NavMenu(int role_type)
        {
            List<frontend_menu> nav = new List<frontend_menu>();

            switch (role_type)
            {
                case 2:
                    nav = db.frontend_menu.OrderBy(f => f.order_num).Where(f => f.student_auth == true).ToList();
                    break;
                case 3:
                    nav = db.frontend_menu.OrderBy(f => f.order_num).Where(f => f.teacher_auth == true).ToList();
                    break;
                case 4:
                    nav = db.frontend_menu.OrderBy(f => f.order_num).Where(f => f.parent_auth == true).ToList();
                    break;
                default:
                    nav = db.frontend_menu.OrderBy(f => f.order_num).Where(f => f.guest_auth == true).ToList();
                    break;
            }

            return nav;
        }

        

        #region UserNav 上方選單控制
        // GET: Navbar
        public ActionResult UserNav(int role_type = 0)
        {
            ViewBag.role_id = new SelectList(db.role, "role_id", "role_desc");

            return PartialView("_UserNavPartial");
        }
        #endregion

        public string GetRole(string id)
        {
            return db.role.Find(id).role_desc;
        }

        public void RoleChange(string role)
        {
            var temp=db.role.Where(r => r.role_desc == role).FirstOrDefault();
            Session["user_role"] = temp.role_id;
            Session["role_desc"] = temp.role_desc;
        }
    }
}