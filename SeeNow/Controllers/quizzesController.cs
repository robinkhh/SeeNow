using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using SeeNow.Models;

namespace SeeNow.Controllers
{
    public class QuizzesController : Controller
    {
        private SeeNowEntities db = new SeeNowEntities();

        //HostStart get QuizzesIndex id parameter = item.group_name
        public ActionResult QuizzesIndex(string id)
        {
            var quiz = from m in db.quizzes
                       select m;
            if (!String.IsNullOrEmpty(id))
            {
                //從quizzes取出包含id的題目
                quiz = quiz.Where(s => s.quiz_group.ToString().Contains(id));
            }
            
            //JOIN 所選題目+4組答案
            var answer_text=(from qz in quiz
                             join qzas in db.quiz_answer on qz.quiz_guid equals qzas.quiz_guid
                             //select new Quiz4Answers { quizzes = qz, quiz_answer = qzas };
                             select new { quizzes = qz, quiz_answer = qzas }).ToList();
            //answer_text內容為title4組+ans4組
            //{"title1":"10+10","ans1":"24","title2":"100+100","ans2":"201","title3":"100+100","ans3":"203"}
            //利用Dictionary<string, object>產出(key,value)
            //strObj.Add會有重覆title key出現,產生錯誤時用catch默許錯誤,只做strObj.Add("ans"..
            //產出的List將只有留{"title":"10+10","ans1":"24","ans2":"201","ans3":"203"}
            int i = 1;
            Dictionary<string, object> strObj = new Dictionary<string, object>();
            List<Dictionary<string, object>> qzansList = new List<Dictionary<string, object>>();
            foreach (var qz in answer_text)
            {
                try
                {
                    strObj.Add("title", qz.quizzes.tittle_text);
                    strObj.Add("time", qz.quizzes.time);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
               
                strObj.Add("ans" + i, qz.quiz_answer.answer_text);
                i++;
                if (i > 4)
                {
                    qzansList.Add(strObj);
                    strObj = new Dictionary<string, object>();
                    i = 1;
                }
            }
            //(key,value)列表序列化Json格式{"title":"10+10","ans1":"24","ans2":"201","ans3":"203"}
            string jsonString = JsonConvert.SerializeObject(qzansList);
            ViewBag.qzans = jsonString;
            return View(qzansList);
        }
       
        // GET: quizzes
        public ActionResult Index()
        {
            var quizzes = db.quizzes.Include(q => q.difficulty_level).Include(q => q.quiz_group1).Include(q => q.type);
            return View(quizzes.ToList());
        }

        // GET: quizzes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            quizzes quizzes = db.quizzes.Find(id);
            if (quizzes == null)
            {
                return HttpNotFound();
            }
            return View(quizzes);
        }

        // GET: quizzes/Create
        public ActionResult Create()
        {
            ViewBag.difficulty_id = new SelectList(db.difficulty_level, "difficulty_id", "difficulty_desc");
            ViewBag.quiz_group = new SelectList(db.quiz_group, "quiz_group1", "group_name");
            ViewBag.type_id = new SelectList(db.type, "type_id", "type_desc");
            return View();
        }

        // POST: quizzes/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "quiz_guid,type_id,tittle_text,title_img_path,title_video_path,difficulty_id,time,score,energy,visible,like_num,quiz_group")] quizzes quizzes)
        {
            if (ModelState.IsValid)
            {
                db.quizzes.Add(quizzes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.difficulty_id = new SelectList(db.difficulty_level, "difficulty_id", "difficulty_desc", quizzes.difficulty_id);
            ViewBag.quiz_group = new SelectList(db.quiz_group, "quiz_group1", "group_name", quizzes.quiz_group);
            ViewBag.type_id = new SelectList(db.type, "type_id", "type_desc", quizzes.type_id);
            return View(quizzes);
        }

        // GET: quizzes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            quizzes quizzes = db.quizzes.Find(id);
            if (quizzes == null)
            {
                return HttpNotFound();
            }
            ViewBag.difficulty_id = new SelectList(db.difficulty_level, "difficulty_id", "difficulty_desc", quizzes.difficulty_id);
            ViewBag.quiz_group = new SelectList(db.quiz_group, "quiz_group1", "group_name", quizzes.quiz_group);
            ViewBag.type_id = new SelectList(db.type, "type_id", "type_desc", quizzes.type_id);
            return View(quizzes);
        }

        // POST: quizzes/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "quiz_guid,type_id,tittle_text,title_img_path,title_video_path,difficulty_id,time,score,energy,visible,like_num,quiz_group")] quizzes quizzes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quizzes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.difficulty_id = new SelectList(db.difficulty_level, "difficulty_id", "difficulty_desc", quizzes.difficulty_id);
            ViewBag.quiz_group = new SelectList(db.quiz_group, "quiz_group1", "group_name", quizzes.quiz_group);
            ViewBag.type_id = new SelectList(db.type, "type_id", "type_desc", quizzes.type_id);
            return View(quizzes);
        }

        // GET: quizzes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            quizzes quizzes = db.quizzes.Find(id);
            if (quizzes == null)
            {
                return HttpNotFound();
            }
            return View(quizzes);
        }

        // POST: quizzes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            quizzes quizzes = db.quizzes.Find(id);
            db.quizzes.Remove(quizzes);
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
