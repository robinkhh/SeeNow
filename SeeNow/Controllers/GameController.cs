using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using SeeNow.Models;

namespace SeeNow.Controllers
{

    public class GameController : Controller
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

            //join quiz_answer 和 quiz 把不是4組答案的題目過瀘,讓題目不出現
            var quiz_quid = (from qzas in db.quiz_answer
                             join qz in quiz on qzas.quiz_guid equals qz.quiz_guid
                             group qz by new { qz.quiz_guid, qz.tittle_text } into g
                             where g.Count() == 4
                             select new { g.Key.quiz_guid, g.Key.tittle_text }).ToList();

            //JOIN 所選題目+4組答案
            //answer_text內容為title4組+ans4組
            //{"title1":"10+10","ans1":"24","title2":"100+100","ans2":"201","title3":"100+100","ans3":"203"}
            //利用Dictionary<string, object>產出(key,value)
            //產出的List將只有留{"title":"10+10","ans1":"24","ans2":"201","ans3":"203"}
            //int i = 1;
            Dictionary<string, object> strObj = new Dictionary<string, object>();
            List<Dictionary<string, object>> qzansList = new List<Dictionary<string, object>>();
            foreach (var q_id in quiz_quid)
            {
                var qz = db.quizzes.Where(q => q.quiz_guid == q_id.quiz_guid)
                           .SingleOrDefault();
                try
                {
                    strObj.Add("quiz_guid", qz.quiz_guid);
                    strObj.Add("title", qz.tittle_text);
                    strObj.Add("img", qz.title_img_path);
                    strObj.Add("time", qz.time);
                    strObj.Add("score", qz.score);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }

                var qzans = db.quiz_answer.Where(q => q.quiz_guid == q_id.quiz_guid)
                   .ToList();
                int i = 1;
                foreach (var qzan in qzans)
                {
                    strObj.Add("ans" + i, qzan.answer_text);
                    strObj.Add("is_correct" + i, qzan.is_correct);
                    i++;
                }
                qzansList.Add(strObj);
                strObj = new Dictionary<string, object>();
            }

            //取出目前round_no最大的一筆,用ViewBag.round_no傳給View用,
            //也可以當Session["round_no"]PIN碼
            var round_no = (from pg in db.play_record
                            orderby pg.round_no.Value descending
                            select pg).First().round_no.Value;

            //play_record新增一筆round_no=目前round_no加1再加上題目列表總數qzansList.Count()
            //再加1,如果有另一群組開題組時題目round_no才不會重覆
            int preserve_round_no = round_no + 1 + qzansList.Count() + 1;
            play_record playRec = new play_record();
            playRec.account = "teacher";
            playRec.score = -1;//先設score = -1
            playRec.datetime = DateTime.Now;
            playRec.quiz_guid = 6;
            playRec.round_no = preserve_round_no;
            db.play_record.Add(playRec);
            db.SaveChanges();

            //(key,value)列表序列化Json格式{"title":"10+10","ans1":"24","ans2":"201","ans3":"203"}
            string jsonString = JsonConvert.SerializeObject(qzansList);
            ViewBag.qzans = jsonString;
            ViewBag.round_no = round_no + 1;
            ViewBag.qzansListCount = qzansList.Count();//題目數量
            Session["round_no"] = ViewBag.round_no;//當PIN用
            return View(qzansList);
        }

        public ActionResult PlayerStart()
        {
            return View();
        }
        public ActionResult HostStart()
        {
            return View(db.quiz_group.ToList());

        }


        public ActionResult ScoreView(string id)
        {
            string[] rounds = id.Split(':');
            string strRound = "";

            List<play_record> allScoreList = new List<play_record>();
            //這一局的所有題組參加的players放入scoreList
            //最前的一組是pin r=1,最後一組空白不要r<=rounds.Length-2
            for (var r = 1; r <= rounds.Length - 2; r++)
            {
                var r_no = int.Parse(rounds[r]);
                var scoreList = (from score in db.play_record
                                 where score.round_no == r_no
                                 select score).ToList();
                strRound += r_no + ";";
                //將scoreList的list累加到allScoreList
                foreach (var all in scoreList)
                {
                    allScoreList.Add(all);
                }
            }
            //scoreList中相同account的score相加,並Descending
            var userSort = allScoreList
            .GroupBy(t => t.account)
            .Select(grp => new { grp.First().account, score = grp.Sum(t => t.score) })
            .OrderByDescending(a => a.score);

            //分數個別寫入users
            foreach (var u in userSort)
            {
                users usr = db.users.Find(u.account);
                usr.score = usr.score + u.score;
                try
                {
                    db.SaveChanges();
                }
                catch (Exception dex)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            //To get all user's profile from the players in this round
            var user_profile_score = (from us in userSort
                                      join u in db.users on us.account equals u.account
                                      join p in db.profile on u.profile_id equals p.profile_id
                                      select new { pf = p.profile_path, acc = u.account,nickname=u.nick_name, sco = us.score })
                                     .ToList();

            //以ViewBag.Round傳前頁面來的Pin碼,當ScoreView頁面開啟時,執行seeScore.js把round填入,並啟動新的signalR
            ViewBag.Round = rounds[0];
            ViewBag.strRound = strRound;
            ViewBag.userSort = user_profile_score;
            return View("ScoreView", allScoreList);
        }

        
        public ActionResult TestView()
        {
            
            Dictionary<string, string> user = new Dictionary<string, string>();
            for (int i = 0; i < 5; i++) { 
            user.Add("name" + i, "user"+i);
            } 
            ViewBag.u_list = user;
            return View();
        }
        public ActionResult QuizView(string id)
        {
            var quiz = from m in db.quizzes
                       select m;
            if (!String.IsNullOrEmpty(id))
            {
                //從quizzes取出包含id的題目
                quiz = quiz.Where(s => s.quiz_group.ToString().Contains(id));
            }


            var quiz_quid = (from qzas in db.quiz_answer
                             join qz in quiz on qzas.quiz_guid equals qz.quiz_guid
                             group qz by new { qz.quiz_guid, qz.tittle_text } into g
                             where g.Count() == 4
                             select new { g.Key.quiz_guid, g.Key.tittle_text }).ToList();
            //int i = 1;
            Dictionary<string, object> strObj = new Dictionary<string, object>();
            List<Dictionary<string, object>> qzansList = new List<Dictionary<string, object>>();
            foreach (var q_id in quiz_quid)
            {
                var qz = db.quizzes.Where(q => q.quiz_guid == q_id.quiz_guid)
                           .SingleOrDefault();
                try
                {
                    strObj.Add("quiz_guid", qz.quiz_guid);
                    strObj.Add("title", qz.tittle_text);
                    strObj.Add("img", qz.title_img_path);
                    strObj.Add("time", qz.time);
                    strObj.Add("score", qz.score);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }

                var qzans = db.quiz_answer.Where(q => q.quiz_guid == q_id.quiz_guid)
                   .ToList();
                int i = 1;
                foreach (var qzan in qzans)
                {
                    strObj.Add("ans" + i, qzan.answer_text);
                    strObj.Add("is_correct" + i, qzan.is_correct);
                    i++;
                }
                qzansList.Add(strObj);
                strObj = new Dictionary<string, object>();
            }

            //JOIN 所選題目+4組答案

            //var answer_text = db.quiz_answer.Join(db.quizzes, qa => qa.quiz_guid, qz => qz.quiz_guid,
            //    (qa, qz) => new {qzid = qz.quiz_guid, title =qz.tittle_text,answer = qa.answer_text,qaid=qa.quiz_guid  });

            string jsonString = JsonConvert.SerializeObject(qzansList);
            ViewBag.qzans = jsonString;
            

            return View();
            
        }
    
    }
}