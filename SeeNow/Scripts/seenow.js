"use strict";

var origin = window.location.origin; 
//audio.play();
var win1_sound = new Audio(origin+'/assets/music/win1.wav');
var ans_snd = new Audio(origin +'/assets/music/symbollongring.mp3');
//var musicArray = new Array("gone_fishin", "deredoc", "dollhouse", "ajourneyawaits", "dream2", "feat_v1", "feat_v2", "spookydungeon", "ANewDay", "AudiomachineBreath", "Psyche", "SoulSwitchbrusspup","TheRockRockHouseJail");
var musicArray = new Array("gone_fishin", "deredoc", "dollhouse", "ajourneyawaits", "dream2", "spookydungeon", "ANewDay", "AudiomachineBreath", "Psyche", "SoulSwitchbrusspup", "TheRockRockHouseJail");
var music = [];//儲存music
var player_account = [];//儲存玩家
var round = [];//儲存回合
//music push to array
for (var i = 0; i < musicArray.length; i++) {
    music.push(new Audio(origin +"/assets/music/" + musicArray[i] + ".mp3"));
}

//產生亂數musicNum
function getSoundNum(maxNum) {
    return Math.floor(Math.random() * maxNum);
}

//播放音效

function playSnd(s) {
    if (s >= 0) {
        music[s].play();
    }
    else {
        for (var i = 0; i < music.length; i++)
            music[i].pause();
    }
}

//暫存題目Json 
var tmpJson = "";

//countProgess
var ctPrgs = 100;
var ctPrgsAvg = 0;
function countProgess(c) {
    if (ctPrgs === 100) {
        $('#countPogress>div').text(c);
        $('#countPogress>div').css({ 'width': ctPrgs + '%' });
        ctPrgsAvg = 100 / c;
        ctPrgs -= ctPrgsAvg;
    }
    else {
        ctPrgs -= ctPrgsAvg;
        $('#countPogress>div').text(c);
        $('#countPogress>div').css({ 'width': Math.floor(ctPrgs) + '%' });
    }
 
}

//倒數計時
var Timer_1;
var timerT = function (c) {
    $('#countTime').html(c);
    $('#countTime').fadeOut(850).fadeIn(10);
    c--;
    Timer_1 = setTimeout(function () {
        if (c <= 0) {
            clearTimeout(Timer_1);
            $('#countTime').html("0");
            playSnd(-1);//-1 for play.pause()
            ans_snd.play();//play Cymbal
            if (tmpJson.redBtn_correct.trim() === "False") {
                //$('#redBtn').animate({ 'opacity': '0.3' }, 500, 'linear');
                $('#redBtn').addClass('btn-no-bg').animate({ 'opacity': '0.3' }, 500, 'linear');
            }
            if (tmpJson.blueBtn_correct.trim() === "False") {
                $('#blueBtn').addClass('btn-no-bg').animate({ 'opacity': '0.3' }, 500, 'linear');
            }
            if (tmpJson.greenBtn_correct.trim() === "False") {
                $('#greenBtn').addClass('btn-no-bg').animate({ 'opacity': '0.3' }, 500, 'linear');
            }
            if (tmpJson.yellowBtn_correct.trim() === "False") {
                $('#yellowBtn').addClass('btn-no-bg').animate({ 'opacity': '0.3' }, 500, 'linear');
            }
        }
        else {
            countProgess(c);
            timerT(c);

        }
    }, 1000);
};

//With the generated proxy
var hubCon = $.connection.gameHub;

//設定Button啟始狀態
//QuizzesIndex disable
$('#sendGroupBtn').attr("disabled", true);
//PlayerStart disable
$('#redBtn').attr("disabled", "true");
$('#blueBtn').attr("disabled", true);
$('#greenBtn').attr("disabled", true);
$('#yellowBtn').attr("disabled", true);


//Start the connection.
$.connection.hub.start().done(function () {
    $('#sendGroupBtn').removeAttr("disabled");
}).catch(function (err) {
    return console.error(err.toString());
});

//加入群組
$('#addGroupBtn').click(function (event) {
    var user = $('#nickname').val();
    var group = $('#group').val();
    $('#autoBtn').removeAttr("disabled");
    var msg2grp = { "group": group, "name": user};
    var msgJSON = JSON.stringify(msg2grp);
    hubCon.server.addGroup(msgJSON).catch(function (err) {
    //hubCon.server.addGroup(group, user).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

//顯示加入訊息0521modified
hubCon.client.recAddGroupMsg = function (msgJSON) {
    var mJson = JSON.parse(msgJSON);
    //var msgTxtarea = $('#message');
    //msgTxtarea.val(msgTxtarea.val() + msg + "\n");
    $('#message').val($('#message').val() +mJson.name + "\n");
    //$('#message').val($('#message').val() + mJson.msg + "\n");
    $('#message').scrollTop($('#message')[0].scrollHeight);

};

//送訊息給群組
$('#sendGroupBtn').click(function () {
    var user = $('#nickname').val();
    var group = $('#group').val();
    var message = $('#msg').val();
    var msg2grp = { "group": group, "name": user, "message": message };
    var msgJSON = JSON.stringify(msg2grp);
    hubCon.server.msgToGroup(msgJSON).catch(function (err) {
        return console.error(err.toString());
    });
});

//接收訊息
hubCon.client.fromGroupMsg = function (msgGroup) {
    //var li = document.createElement("li");
    //li.textContent = msg+"&#13;&#10;";
    //$('#message').append(li);
    var msgTxtarea = $('#message');
    msgTxtarea.val(msgTxtarea.val() + msgGroup + "\n");
    $('#message').scrollTop($('#message')[0].scrollHeight);
};

//接收分數
hubCon.client.groupScore = function (msgGroup) {
    //var li = document.createElement("li");
    //li.textContent = msg+"&#13;&#10;";
    //$('#message').append(li);
    $('#quizDiv').html(msgGroup);
};

//接收題目
hubCon.client.receiveGroupQuiz = function (msgJSON) {
    //有接收題目,才enable答題button
    $('#redBtn').removeAttr("disabled").removeClass('btn-no-bg');
    $('#blueBtn').removeAttr("disabled").removeClass('btn-no-bg');
    $('#greenBtn').removeAttr("disabled").removeClass('btn-no-bg');
    $('#yellowBtn').removeAttr("disabled").removeClass('btn-no-bg');
    //reset time bar
    ctPrgs = 100;
    //
    var mJson = JSON.parse(msgJSON);
    tmpJson = mJson;
    //push round_no to round at client site 
    round.push(tmpJson.round_no);
    //tmpJson.title
    //$('#quizDiv').html("<img src=\"../assets/image/" + mJson.img
    //    + "\" onerror='this.src = \"../assets/image/teacher2-school.gif\"' style=\"width:260px;height:260px;\" /> "
    //    + mJson.title);
    
    $('#quizDiv').html("<img src=\"" + origin+"/assets/image/" + mJson.img
        + "\" onerror='this.src = \"" + origin+"/assets/image/teacher2-school.gif\"' style=\"width:260px;height:260px;\" /> "
        + mJson.title);
    //tmpJson.interval_Time
    $('#countTime').html(mJson.interval_Time);//countTime顯示interval_Time
    timerT(parseInt($('#countTime').html()));//timerT(c),c=countTime
   // $('#message').html(mJson.msg);
    $('#redBtn').val(mJson.redBtn).css({ 'opacity': '0.9' });
    $('#blueBtn').val(mJson.blueBtn).css({ 'opacity': '0.9' });
    $('#greenBtn').val(mJson.greenBtn).css({ 'opacity': '0.9' });
    $('#yellowBtn').val(mJson.yellowBtn).css({ 'opacity': '0.9' });
    playSnd(mJson.sndNum);
    
};


function playQuiz(startNum, endNum, round_no) {
    
    var tbRows = $('#Tbquiz').find('tbody').find('tr');
    var group = $('#group').val();
    var user = $('#name').val();
    var redBtn = 0, blueBtn = 0, greenBtn = 0, yellowBtn = 0;
    var redBtn_correct, blueBtn_correct, greenBtn_correct, yellowBtn_correct;
    var qtimer;
    var sndNum;
    var startRow;
    var endRow;
    var rightAns;
    //(startNum !== endNum)autoPlay,(startNum=endNum) Play quiz by selected
    if (startNum !== endNum) { startRow = startNum; endRow = tbRows.length - 1; }
    else { startRow = startNum; endRow = endNum;}
    function q_display() {
        if (startRow <= endRow) {
            //使用過的題目,按鈕變無效
            $("#menuBtn" + startRow).attr("disabled", true);
            //編號
            var quiz_guid = $(tbRows[startRow]).find('td:eq(0)').text().trim();
            //題目
            var title = $(tbRows[startRow]).find('td:eq(1)').text().trim();
            //從QuizIndex取id="interval_Time"題目間隔時間
            //var interval_Time = $('#interval_Time').val();
            var interval_Time = $(tbRows[startRow]).find('td:eq(2)').text().trim();
            //答案A
            redBtn = $(tbRows[startRow]).find('td:eq(3)').text().trim();
            //is_correct1
            redBtn_correct = $(tbRows[startRow]).find('td:eq(4)').text().trim();
             if(redBtn_correct.trim() === "True")rightAns=1;
            //答案B
            blueBtn = $(tbRows[startRow]).find('td:eq(5)').text().trim();
            //is_correct2
            blueBtn_correct = $(tbRows[startRow]).find('td:eq(6)').text().trim();
            if (blueBtn_correct.trim() === "True") rightAns = 2;
            //答案C
            greenBtn = $(tbRows[startRow]).find('td:eq(7)').text().trim();
            //is_correct3
            greenBtn_correct = $(tbRows[startRow]).find('td:eq(8)').text().trim();
            if (greenBtn_correct.trim() === "True") rightAns = 3;
            //答案D
            yellowBtn = $(tbRows[startRow]).find('td:eq(9)').text().trim();
            //is_correct4
            yellowBtn_correct = $(tbRows[startRow]).find('td:eq(10)').text().trim();
            if (yellowBtn_correct.trim() === "True") rightAns = 4;
            var score = $(tbRows[startRow]).find('td:eq(11)').text().trim();
            var img = $(tbRows[startRow]).find('td:eq(12)').text().trim();
            //rowid
            //rowid = $(tbRows[startRow]).find('td:eq(11)').html();
            sndNum = getSoundNum(musicArray.length);//Get a random number for play sound
            var msg2grp = {
                "group": group,
                "name": user,
                "sndNum": sndNum,
                "quiz_guid": quiz_guid,
                "title": title,
                "interval_Time": interval_Time,
                "redBtn": redBtn,
                "redBtn_correct": redBtn_correct,
                "blueBtn": blueBtn,
                "blueBtn_correct": blueBtn_correct,
                "greenBtn": greenBtn,
                "greenBtn_correct": greenBtn_correct,
                "yellowBtn": yellowBtn,
                "yellowBtn_correct": yellowBtn_correct,
                "rowid": startRow,
                "round_no": round_no,
                "rightAns": rightAns,
                "score": score,
                "img": img
            };
            var msgJSON = JSON.stringify(msg2grp);
            hubCon.server.sendQuizToGroup(msgJSON).catch(function (err) {
                return console.error(err.toString());
            });
            //如果是連續題,要增加round_no和row開始新題目
            ++round_no;
            startRow++;
            //*1100 多0.1秒讓計時器timerT有足夠顯示0的時間
            //interval_Time+4000 顯示答案
            qtimer = setTimeout(q_display, interval_Time * 1000 + 4000);
        }   

        else {
            //題組結束,停止計時
            clearTimeout(qtimer);
            playSnd(-1);//-1停止music
            //顯示得分
            msg2grp = { "group": group, "round": round, "endRow": endRow };
            msgJSON = JSON.stringify(msg2grp);
            hubCon.server.roundScore(msgJSON).catch(function (err) {
                return console.error(err.toString());
            });

        }
    }
    q_display();
}


//QuizzesIndex按下endRoundBtn,將group和round array帶到下頁
function endRound() {
    var msg2grp = { "group": $('#group').val(), "round": round};
    var msgJSON = JSON.stringify(msg2grp);
    hubCon.server.endRound(msgJSON).catch(function (err) {
        return console.error(err.toString());
    });
}


//接收4個答案鈕
function choicBtn(choic) {
    //按下答題disable button
    $('#redBtn').attr("disabled", "disabled");
    $('#blueBtn').attr("disabled", "disabled");
    $('#greenBtn').attr("disabled", "disabled");
    $('#yellowBtn').attr("disabled", "disabled");
    var user = $('#name').val();
    var group = $('#group').val();//
    var select_answer = choic.name;//取得choicBtn的name 1,2,3,4
    //tmpJson為QuizzesIndex送來Quiz時暫存檔,供client利用
    //tmpJson.quiz_guid;
    var msg2grp = {
        "round_no": tmpJson.round_no,
        "name": user,
        "select_answer": select_answer,
        "quiz_guid": tmpJson.quiz_guid,
        "rightAns": tmpJson.rightAns,
        "score": tmpJson.score
    };
    var msgJSON = JSON.stringify(msg2grp);
    hubCon.server.play_answer(msgJSON).catch(function (err) {
                return console.error(err.toString());
    });
    //Show User Selected answer
    msg2grp = {
        "group": group,
        "name": $('#nickname').val(),
        "select_answer": select_answer,
        "quiz_guid": tmpJson.quiz_guid
    };
     msgJSON = JSON.stringify(msg2grp);
    hubCon.server.msgToGroup(msgJSON).catch(function (err) {
        return console.error(err.toString());
    });
}

