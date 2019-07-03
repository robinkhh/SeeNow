using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SeeNow.Models;

namespace SeeNow
{
    public class GameHub : Hub
    {
        SeeNowEntities db = new SeeNowEntities();
        
        //public void Send(string name, string message)
        //{
        //    // Call the addNewMessageToPage method to update clients.
        //    Clients.All.addNewMessageToPage(name, message);
        //}
        public void InitalConnt()
        {
            Clients.Caller.initalConnt(DateTime.Now.ToShortTimeString(), "ZZ" + Context.ConnectionId);
        }
        public async Task AddGroup(string msgJSON)
        {
            dynamic msg = JsonConvert.DeserializeObject(msgJSON);
            await Groups.Add(Context.ConnectionId, (string)msg.group);
            await Clients.Group((string)msg.group).RecAddGroupMsg(msgJSON);
        }


        public Task SendQuizToGroup(string msgJSON)
        {
            dynamic msg = JsonConvert.DeserializeObject(msgJSON);
            return Clients.Group((string)msg.group).ReceiveGroupQuiz(msgJSON);
        }

        public async Task MsgToGroup(string msgJSON)
        {
            dynamic msg = JsonConvert.DeserializeObject(msgJSON);
            //await Clients.Group((string)msg.group).FromGroupMsg(msg.group + "::" + msg.name + ":" + msg.select_answer + msg.message);
            await Clients.Group((string)msg.group).FromGroupMsg(msg.name + ":" + msg.select_answer + msg.message);

        }
        public void Play_answer(string msgJSON)
        {

            JObject msgJson = JObject.Parse(msgJSON);
            int intRound_no = msgJson["round_no"].Value<int>();
            string strName = msgJson["name"].Value<string>();
            int intQuiz_guid = msgJson["quiz_guid"].Value<int>();
            int intSelect_answer = msgJson["select_answer"].Value<int>();
            int intRightAns = msgJson["rightAns"].Value<int>();
            int intScore = msgJson["score"].Value<int>();
            play_record_answer plyRecAns;
            //round_no由GameController抽出player_record最後一筆的round_no+1,
            //按下choicBtn鈕時測試有無round_no和group(PIN)
            //測試有没有round_no
            var playRec_round_no = from r in db.play_record
                                   where r.round_no == intRound_no
                                   select r;
            //用playRec_round_no 的結果測試round_no回合有無剛加入的"name",
            //如果返回null表示"name"新加入,需新增一筆play_record給新加人者
            var play_account = playRec_round_no.FirstOrDefault(a => a.account == strName);

            //無回合round_no, 或有round_no但是name是新加入的都要新增一筆play_record
            if (!playRec_round_no.Any() || (play_account == null))
            {
                play_record playRec = new play_record();
                playRec.account = strName;
                playRec.score = 0;//先設score = -1
                playRec.datetime = DateTime.Now;
                playRec.quiz_guid = intQuiz_guid;
                playRec.round_no = intRound_no;
                db.play_record.Add(playRec);
                db.SaveChanges();

                //取新增的play_record寫入對應的play_record_answer
                var ply_gid = (from pg in db.play_record
                               where pg.account == strName
                               && pg.quiz_guid == intQuiz_guid
                               && pg.round_no == intRound_no
                               select pg).FirstOrDefault();

                plyRecAns = new play_record_answer();
                plyRecAns.play_guid = ply_gid.play_guid;
                plyRecAns.account = ply_gid.account;
                plyRecAns.select_answer = intSelect_answer;
                plyRecAns.quiz_guid = ply_gid.quiz_guid;
                db.play_record_answer.Add(plyRecAns);
                db.SaveChanges();
                if (intRightAns == intSelect_answer)
                {
                    playRec.score = intScore;
                    db.SaveChanges();
                }
            }
            //else {
            //    var ply_ans = (from plans in db.play_record_answer
            //                  join plrec in db.play_record on plans.play_guid equals plrec.play_guid
            //                  select plans).FirstOrDefault();
            //    ply_ans.select_answer = intSelect_answer;
            //    db.SaveChanges();
            //}

        }


        public async Task RoundScore(string msgJSON)
        {
            dynamic msg = JsonConvert.DeserializeObject(msgJSON);
            var endRow = msg.endRow;

            Dictionary<string, int> intTotalScore = new Dictionary<string, int>();
            foreach (int r in msg.round)
            {
                var playRec = (from u in db.users
                                  join p in db.play_record
                                 on u.account equals p.account
                                  where p.round_no == r
                                  select new {  u.account, u.nick_name, p.score }).ToList();



                //var playRec = (from p in db.play_record
                //               where p.round_no == r
                //               select p).ToList();

                var strPlayRec = "";
                var strTotalPlayRec = "";

                foreach (var p in playRec)
                {
                    //顯示單次成績
                    //strPlayRec += "<div>"+p.round_no + ":" + p.account + ":+" + p.score + "</div>";
                    //strPlayRec += "<div>" + p.account + ":+" + p.score + "</div>";
                    strPlayRec += "<div>" + p.nick_name + ":+" + p.score + "</div>";
                    //新增一筆(p.account,value)
                    try
                    {
                        intTotalScore.Add(p.account, p.score);
                    }
                    catch (Exception ex)
                    {
                        //把Key的值取出加這次分數
                        intTotalScore[p.account] += p.score;
                    }
                }
                foreach (var s in intTotalScore)
                {
                    strTotalPlayRec += "<div>" + s + "</div>";
                    //strTotalPlayRec += "<div><script>confirm(location.href = 'http://localhost:63733/Game/ScoreView');</script></div>";
                }
                //endRow-1表示連續題,只顯示Total
                if (endRow == "-1")
                {
                    await Clients.Group((string)msg.group).GroupScore(strPlayRec);
                }
                //非連續題,顯示各別得分和各別總分
                else
                {
                    var request = HttpContext.Current.Request;
                    //await Clients.Group((string)msg.group).GroupScore(strPlayRec + " Total:" + strTotalPlayRec);
                    //await Clients.Group((string)msg.group).GroupScore("<div><img src = '../assets/image/people-dancing.gif' /></div>" + strPlayRec);
                    await Clients.Group((string)msg.group).GroupScore
                        ("<div><img src = '"+request.Url.GetLeftPart(UriPartial.Authority)+"/assets/image/people-dancing.gif' /></div>"
                        + strPlayRec);
                }
            }

        }

        //回合結束,在QuizzesIndex中按endRoundBtn轉至新頁面/Game/ScoreView
        public async Task EndRound(string msgJSON)
        {
            dynamic msg = JsonConvert.DeserializeObject(msgJSON);
            var rounds = "";//To gather all rounds for next page using
            foreach (var r in msg.round)
            {
                rounds += r + ":";
            }
            //Redirect url to /Game/ScoreView
            var grpRound = msg.group + ":" + rounds;//準備頁面id參數/Game/ScoreView/?id=
            var request = HttpContext.Current.Request;
            var reDirUrl = "<div><script>confirm(location.href = '" + request.Url.GetLeftPart(UriPartial.Authority) + "/Game/ScoreView?id=" + grpRound + "');</script></div>";
            await Clients.Group((string)msg.group).GroupScore(reDirUrl);
        }
        public async Task SendGift(string msgJSON)
        {
            dynamic msg = JsonConvert.DeserializeObject(msgJSON);
            //await Clients.Group((string)msg.group).FromGroupMsg(msg.group + "::" + msg.name + ":" + msg.select_answer + msg.message);
            await Clients.Group((string)msg.group).RcvGift(msgJSON);

        }

    }
}
