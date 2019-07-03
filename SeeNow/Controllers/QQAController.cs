using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using SeeNow.Models;
using SeeNow.ViewModels;

namespace SeeNow.Controllers
{
    
    public class QQAController : Controller
    {
        SeeNowEntities db = new SeeNowEntities();

        // GET: QQA，完成
        public ActionResult Index(int? id)
        {
            if (id == null)
                id = 12;

            QQA qqa = new QQA();

            qqa.quizzes = db.quizzes.OrderBy(q=>q.quiz_group).ToList();

            var quizzes_list = from a in db.quizzes
                               join b in db.quiz_group on a.quiz_group equals b.quiz_group1
                               orderby a.quiz_group
                               select new { a.tittle_text, b.group_name ,a.quiz_guid };
            ViewBag.quizzes_list = quizzes_list;

              qqa.answers = db.quiz_answer.Where(m => m.quiz_guid == id).ToList();
            //qqa.quiz = db.quizzes.Where(m => m.quiz_guid == id).ToList();
            //qqa.quiz_Groups=db.quiz_group.Where(m=>m.quiz_group1==q.)


            ViewBag.qzid = id;
            ViewBag.Ttext = db.quizzes.Where(m => m.quiz_guid == id).FirstOrDefault();
            return View(qqa);
            

        }

        // GET: QGQA，完成
        public ActionResult QGQAIndex(int id = 14)
        {

            //QQA qgqa = new QQA();

            //ViewBag.quizzes= db.quizzes.Where(m => m.quiz_group == id).ToList();
            //ViewBag.quizans = db.quiz_answer.Where(m => m.quiz_guid ==).ToList();
            
            var quiz_Groups = db.quiz_group.ToList();
            //var quizzes = db.quizzes.Where(m => m.quiz_group == id).ToList();
            //qgqa.answers = db.quiz_answer.ToList();
            //var answers = db.quiz_answer.ToList();


            ViewBag.quizzes = from c in db.quizzes
                              join s in db.quiz_answer on c.quiz_guid equals s.quiz_guid
                              where c.quiz_group==id
                         select new { c.quiz_guid, c.type_id,c.tittle_text,c.time,c.score,c.quiz_group,
                         s.answer_id,s.answer_text,s.is_correct
                         };
           

            //foreach(var item in qgqa.quizzes)
            //{
            //    ViewBag.quiz_guid = qgqa.quizzes[0];
            //}
            //ViewBag.quizzes = result;
            //ViewBag.answers = qgqa.answers;

            return View(quiz_Groups);


        }
        public ActionResult QuizzesIndex(int id = 12)
        {
            //Object[] str = new object[10];
            Dictionary<string, object> strObj = new Dictionary<string, object>();
            List<Dictionary<string, object>> qzansList = new List<Dictionary<string, object>>();
            int i = 1;

            QQA qqa = new QQA();

            qqa.quiz_Groups = db.quiz_group.ToList();
            qqa.answers = db.quiz_answer.ToList();
            //qqa.answers = db.quiz_answer.Where(m => m.quiz_guid == id).ToList();
            qqa.quiz = db.quizzes.Where(m => m.quiz_guid == id).ToList();

            strObj.Add("title", qqa.quiz[0].tittle_text);   //string
            strObj.Add("time", qqa.quiz[0].time);       //int

            foreach (var j in qqa.answers)
            {
                strObj.Add("ans" + i, j.answer_text);
                strObj.Add("iscorrect" + i, j.is_correct);
                i++;
                //if (i > 4)
                //{
                //    qzansList.Add(strObj);
                //    strObj = new Dictionary<string, object>();
                //    i = 1;
                //}
            }
            //strObj.ToList();

            //(key,value)列表序列化Json格式{"title":"10+10","ans1":"24","ans2":"201","ans3":"203"}
            string jsonString = JsonConvert.SerializeObject(strObj);
            ViewBag.qzans = jsonString;

            return View(qqa);


        }

        //20180530新增，新增題目群組，完成
        public ActionResult QGAdd()
        {

            //return View();
            //return View(db.quiz_group.GroupBy(m=>m.category_id).ToList());      //有錯誤
            //20190610新增
            ViewBag.category = new SelectList(db.category,"category_id", "category_desc");
            return View(db.quiz_group.ToList());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult QGAdd(int category_id, string group_name, string account )
        {
            var gname = db.quiz_group.Where(m => m.group_name == group_name).FirstOrDefault();
            
            if (gname == null)
            {
                var qgid = db.quiz_group.OrderByDescending(m => m.quiz_group1).FirstOrDefault();

                quiz_group qg = new quiz_group();
                qg.category_id = category_id;
                qg.quiz_group1 = qgid.quiz_group1+1;

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

        //20180604新增，修改題目群組，完成
        public ActionResult QGEdit(int? id)
        {

            //return View();
            return View(db.quiz_group.Where(m => m.quiz_group1 == id).FirstOrDefault());
            //return View(db.tProduct.Where(m => m.fId == id).FirstOrDefault());
        }

        //20180604新增，題目列表，完成
        public ActionResult QZIndex()
        {
            return View(db.quizzes.ToList());
        }

        //20180604新增，答案列表，完成
        public ActionResult QAIndex()
        {
            return View(db.quiz_answer.ToList());
        }

        //20180606新增，新增題目，完成
        public ActionResult QZAdd()
        {

            //return View();
            //20190610新增，完成
            ViewBag.difficulty_desc = new SelectList(db.difficulty_level, "difficulty_id", "difficulty_desc");
            ViewBag.group_name = new SelectList(db.quiz_group, "quiz_group1", "group_name",22);
            //ViewBag.group_name = new SelectList(db.quiz_group, "quiz_group1", "group_name", db.quiz_group.Where(m=>m.group_name=="綜合題").First().group_name);
            return View(db.quizzes.ToList());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult QZAdd(int group_name, string tittle_text, int time, short score, int energy, bool visible, short difficulty_desc, string type_id)
        {

            var tittletext = db.quizzes.Where(m => m.tittle_text == tittle_text).FirstOrDefault();
            //difficulty_level diffid = db.difficulty_level.Where(m => m.difficulty_desc == difficulty_desc).FirstOrDefault();

            if (tittletext == null)
            {
                var qzid = db.quizzes.OrderByDescending(m => m.quiz_guid).FirstOrDefault();

                quizzes qz = new quizzes();

                qz.quiz_guid = qzid.quiz_guid + 1;

                qz.quiz_group = group_name;
                qz.tittle_text = tittle_text;
                qz.time = time;
                qz.score = score;
                qz.energy = energy;
                qz.visible = visible;

                qz.difficulty_id = difficulty_desc;
                qz.type_id = "1";

                db.quizzes.Add(qz);
                db.SaveChanges();

                return RedirectToAction("Index", "QQA");
            }
            ViewBag.Message = "遊戲題組名稱，已有人使用!!";
            return View(db.quizzes.ToList());


        }

        //20180611新增，修改題目
        public ActionResult QZEdit(int? id)
        {
            

            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            quizzes qz = db.quizzes.Find(id);
            if (qz == null)
            {
                return HttpNotFound();
            }

            ViewBag.difficulty_desc = new SelectList(db.difficulty_level, "difficulty_id", "difficulty_desc",qz.difficulty_id);
            ViewBag.group_name = new SelectList(db.quiz_group, "quiz_group1", "group_name",qz.quiz_group);

            return View(qz);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult QZEdit(int quiz_guid, string tittle_text,short difficulty_desc, int time, short score, int energy, bool visible ,int group_name)
        {
            quizzes data = db.quizzes.Where(d => d.quiz_guid == quiz_guid).FirstOrDefault();
            data.tittle_text = tittle_text;
            data.difficulty_id = difficulty_desc;
            data.time = time;
            data.score = score;
            data.energy = energy;
            data.visible = visible;
            data.quiz_group = group_name;
            db.SaveChanges();
            return RedirectToAction("Index", "QQA");

        }

        // 20190613新增，完成
        //新增答案，只列出沒有答案的題目
        public ActionResult QAAdd(int? id)
        {
            quizzes qz = db.quizzes.Where(m => m.quiz_guid == id).FirstOrDefault();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.QZID = id;
            ViewBag.Ttext = qz.tittle_text;
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult QAAdd(int id, string answer_text, bool is_correct)
        {
            quiz_answer qa = new quiz_answer();
            var qaid = db.quiz_answer.Where(m => m.quiz_guid == id).ToList();

            if (qaid.Count > 4)
            {
                ViewBag.Message = "遊戲答案數量過多!!";
            }
            else
            {
                qa.quiz_guid = id;
                qa.type_id = "1";
                qa.answer_text = answer_text;
                qa.is_correct = is_correct;

                db.quiz_answer.Add(qa);
                db.SaveChanges();

            }

            return RedirectToAction("Index", new {id});
            
        }

        //20180614新增，修改答案，
        public ActionResult QAEdit(int? qzid, int? qaid)
        {
            quiz_answer qa = db.quiz_answer.Where(m => m.quiz_guid == qzid && m.answer_id==qaid).FirstOrDefault();
            quizzes qz = db.quizzes.Where(m => m.quiz_guid == qzid).FirstOrDefault();

            if (qzid == null || qaid==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.QZID = qzid;
            ViewBag.QAID = qaid;
            ViewBag.Ttext = qz.tittle_text;

            ViewBag.QZ = db.quizzes.Where(m => m.quiz_guid == qzid).FirstOrDefault();

            return View(qa);
            //return RedirectToAction("Index", new { id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult QAEdit(int quiz_guid, int answer_id, string answer_text, bool is_correct)
        {
            quiz_answer qaid = db.quiz_answer.Where(m => m.quiz_guid == quiz_guid && m.answer_id==answer_id).FirstOrDefault();

            qaid.type_id = "1";
            qaid.answer_text = answer_text;
            qaid.is_correct = is_correct;

            var id = quiz_guid;
            db.SaveChanges();
            

            return RedirectToAction("Index", new { id });

        }

        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    quiz_answer qa = db.quiz_answer.Find(id);
        //    if (qa == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    return View("Delete", qa);
        //}


    }
}