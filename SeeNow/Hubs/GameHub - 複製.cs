using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SeeNow.Models;
using SeeNow.Hubs;
using SeeNow.Hubs.Models;
using Microsoft.AspNet.SignalR.Hubs;

namespace SeeNow
{
    
    public class GameHub : Hub
    {
        SeeNowEntities db = new SeeNowEntities();

        public static DbContext dbS = new DbContext();

        /// <summary>
        /// 重写Hub连接事件
        /// </summary>
        /// <returns></returns>
        #region OnConnected
        public override Task OnConnected()
        {
            //查询用户
            var user = dbS.Users.Where(w => w.UserName == Context.ConnectionId).FirstOrDefault();
            //判断用户是否存在
            if (user == null)
            {
                user = new User()
                {
                    UserName = Context.ConnectionId
                };
                dbS.Users.Add(user);
            }
            //发送房间列表
            var rooms = dbS.Rooms.Select(p => p.RoomName).ToList();
            //注册getRooms 获取房间的方法
            Clients.Client(Context.ConnectionId).getRoomList(JsonConvert.SerializeObject(rooms));
            return base.OnConnected();
        }
        #endregion
        //更新所有用户的房间列表
        #region GetRooms
        private void GetRooms()
        {
            var rooms = JsonConvert.SerializeObject(dbS.Rooms.Select(p => p.RoomName).ToList());
            Clients.All.getRoomList(rooms);
        }
        #endregion
        //重写Hub链接断开事件
        #region OnDisconnected
        public override Task OnDisconnected(bool s)
        {
            var user = dbS.Users.Where(u => u.UserName == Context.ConnectionId).FirstOrDefault();
            //判断用户是否存在，存在则删除
            if (user != null)
            {
                //删除用户
                dbS.Users.Remove(user);

            }
            return base.OnDisconnected(s);
        }
        #endregion
        //加入聊天室
        #region AddRoom
        public void AddRoom(string roomName)
        {
            //查询聊天室
            var room = dbS.Rooms.Find(a => a.RoomName == roomName);
            //存在则加入
            if (room != null)
            {
                //查找房间中是否存在此用户
                var isUser = room.Users.Where(w => w.UserName == Context.ConnectionId).FirstOrDefault();
                //不存在则加入
                if (isUser == null)
                {
                    var user = dbS.Users.Find(a => a.UserName == Context.ConnectionId);
                    user.Rooms.Add(room);
                    room.Users.Add(user);
                    Groups.Add(Context.ConnectionId, roomName);
                    //注册加入聊天室的addRoom方法
                    Clients.Client(Context.ConnectionId).addRoom(roomName);
                }
                else
                {
                    Clients.Client(Context.ConnectionId).showMessage("請勿重複加入遊戲空間");
                }
            }
        }
        #endregion
        //创建聊天室
        #region CreateRoom
        public void CreateRoom(string roomName)
        {
            var room = dbS.Rooms.Find(a => a.RoomName == roomName);
            if (room == null)
            {
                Room r = new Room() { RoomName = roomName };
                //将房间加入列表
                dbS.Rooms.Add(r);
                AddRoom(roomName);
                Clients.Client(Context.ConnectionId).showMessage("遊戲空間創建完成");
                GetRooms();
            }
            else
            {
                Clients.Client(Context.ConnectionId).showMessage("遊戲空間名稱重複");
            }
        }
        #endregion
        //退出聊天室
        #region ExitRoom
        public void ExitRoom(string roomName)
        {
            //查找房间是否存在
            var room = dbS.Rooms.Find(a => a.RoomName == roomName);
            //存在则删除
            if (room != null)
            {
                //查找要删除的用户
                var user = room.Users.Where(p => p.UserName == Context.ConnectionId).FirstOrDefault();
                //移除此用户
                room.Users.Remove(user);
                //如果房间人数为0，怎删除房间
                if (room.Users.Count == 0)
                {
                    dbS.Rooms.Remove(room);
                }
                //Groups Remove移除分组方法
                Groups.Remove(Context.ConnectionId, roomName);
                //提示客户端
                Clients.Client(Context.ConnectionId).removeRoom("成功退出遊戲空間");
                
            }
            GetRooms();
        }
        #endregion
        //给分组内所有用户发送消息
        #region SendMsg
        public void SendMsg(string Room, string Message)
        {
            Clients.Group(Room, new string[0]).sendMessage(Room, Message + " " + DateTime.Now.ToString());
        }
        #endregion




        private static List<Player> playerList = new List<Player>();//紀錄連線者
        private static Player player;

        /// <summary>
        /// 產生四碼不重複數字
        /// </summary>
        /// <returns></returns>
        #region CreateCode
        public string CreateCode()
        {
            Random rnd = new Random();
            string code = rnd.Next().ToString().Substring(0, 6);
            do
            {
                code = rnd.Next().ToString().Substring(0, 6);
            } while (playerList.Where(p => p.Code == code).Count() > 0);
            //先移除上次產生的號碼，如果有的話
            playerList.RemoveAll(p => p.ConnectionID == Context.ConnectionId);
            //要求產生號碼時，紀錄連線的 ConnectionId，還有上面產生的四個不重複數字
            playerList.Add(new Player { ConnectionID = Context.ConnectionId, Code = code });
            //把資訊傳遞到 client 的 getCode 事件中
            return Clients.Client(Context.ConnectionId).getCode(code);
            //如果你要指定是哪個 Client 裝置，就要用 Clients.Client(連線的編號) 去指定
            //如果是全部的 Client 都要，那可以用 Clients.All
            //不管是 Clients.Client(連線的編號)，還是 Clients.All，後面接的都是在 client 端 js 的方法名稱
            //以下面這段為例：
            //Clients.Client(Context.ConnectionId).getCode(code);
            //            getCode 指的就是傳遞到 Client js 的 getCode方法
            //如果你第一個字是大寫的 GetCode，到了 client js 第一個字要記得小寫
        }
        #endregion

        /// <summary>
        /// 配對
        /// 如果輸入的數字與記錄中的編號相符，且還沒有進行連線的話，就會呼叫兩端的 matchMsg 確認連線
        /// 如果號碼相符但已經連線過了，就會對要求連線的裝置回傳 false
        /// </summary>
        /// <param name="code"></param>
        #region FindSameCode
        public void FindSameCode(string code)
        {
            player = playerList.FirstOrDefault(p => p.Code == code && p.Code2 == null);
            if (player == null)
            {
                Clients.Client(Context.ConnectionId).matchMsg(false);
            }
            else
            {
                player.ConnectionID2 = Context.ConnectionId;
                player.Code2 = code;
                Clients.Client(Context.ConnectionId).matchMsg(true);
                Clients.Client(player.ConnectionID).matchMsg(true);
            }
        }
        #endregion

        //public void Send(string name, string message)
        //{
        //    // Call the addNewMessageToPage method to update clients.
        //    Clients.All.addNewMessageToPage(name, message);
        //}
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
            await Clients.Group((string)msg.group).FromGroupMsg(msg.group + "::" + msg.name + ":" + msg.select_answer + msg.message);
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
            //round_no由QuizzesIndexController抽出player_record最後一筆的round_no+1,
            //按下鈕時測試有無round_no和group
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
                var playRec = (from p in db.play_record
                               where p.round_no == r
                               select p).ToList();

                var strPlayRec = "";
                var strTotalPlayRec = "";
                foreach (var p in playRec)
                {
                    //顯示單次成績
                    //strPlayRec += p.round_no + ": " + p.account + " +" + p.score;
                    strPlayRec += "<div>" + p.account + ": +" + p.score + "</div>";
                    
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
                foreach (var s in intTotalScore) {
                    strTotalPlayRec += "<div>" +s+"</div>";
                    //strTotalPlayRec += "<div><script>confirm(location.href = 'http://localhost:63733/Game/ScoreView');</script></div>";
                }
                //endRow-1表示連續題,只顯Total,非連續題
                if (endRow == "-1")
                {
                    await Clients.Group((string)msg.group).GroupScore(strPlayRec);
                }
                else
                {
                    await Clients.Group((string)msg.group).GroupScore(strPlayRec + " Total:" + strTotalPlayRec);
                }
            }

        }
        public async Task EndRound(string group)
        {
            //Redirect url to /Game/ScoreView
            var request = HttpContext.Current.Request;
            var redir = "<div><script>confirm(location.href = '"+ request.Url.GetLeftPart(UriPartial.Authority) + "/Game/ScoreView');</script></div>";
            await Clients.Group((string)group).GroupScore(redir);
        }
        
    }
}
