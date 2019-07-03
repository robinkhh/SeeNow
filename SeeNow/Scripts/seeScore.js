//1,將連線打開
(function () {
    openCon();
})();
//2,建立與Server端的Hub的物件，注意Hub的開頭字母一定要為小寫
var hubCon = $.connection.gameHub;
function openCon() {
    //將連線打開
    $.connection.hub.start().done(function () {
        //3,加入群組
        addGroupBtn();
    }).fail(function () {
        alert("發生錯誤");
    });
}

//3,加入群組
function addGroupBtn() {
    var user = $('#nickname').val();
    var group = $('#group').val();
    $('#autoBtn').attr("disabled", false);
    var msg2grp = { "group": group, "name": user };
    var msgJSON = JSON.stringify(msg2grp);
    hubCon.server.addGroup(msgJSON).catch(function (err) {
        //hubCon.server.addGroup(group, user).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
}
//4,顯示加入訊息
hubCon.client.recAddGroupMsg = function (msgJSON) {
    var mJson = JSON.parse(msgJSON);
    //var msgTxtarea = $('#message');
    //msgTxtarea.val(msgTxtarea.val() + msg + "\n");
    $('#message').val($('#message').val() + mJson.name + "\n");
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

//設定主機網址
var origin = window.location.origin;
var data;
//設定拖拉道具產生效果
//document.ondragstart = function (event) {
//    event.dataTransfer.setData("Text", event.target.id);
//    document.getElementById("demo").innerHTML = "Started to drag the: " + event.target.id;
//};
document.addEventListener("dragstart", function (event) {
    // The dataTransfer.setData() method sets the data type and the value of the dragged data
    //save被抓取元素的id(為string值)
    //event.dataTransfer.setData("Text", event.target.id);
    //event.dataTransfer.setData("Text", event.target.innerHTML);
    event.dataTransfer.setData("Text", $(event.target).attr("title"));
    // Output some text when starting to drag the p element
    //document.getElementById("info").innerHTML = "Started to drag the p element.";
    // Change the opacity of the draggable element
    //event.target.style.opacity = "0.4";
});

document.ondragend = function (event) {

    event.preventDefault();
    //if (event.target.className === "droptarget") {
    //    var data = event.dataTransfer.getData("Text");
    //    document.getElementById("info").innerHTML = data + " : " + event.target.id;

    //}
};

/* Events fired on the drop target */
document.ondragover = function (event) {
    event.preventDefault();
};

document.ondrop = function (event) {
    event.preventDefault();
    if (event.target.className === "droptarget") {
        data = event.dataTransfer.getData("Text");
        //event.target.appendChild(document.getElementById(data));
        document.getElementById("info").innerHTML = $('#my_nickname').html() +"送"+data+"給" + event.target.id+"! ";
        //送禮物
        sendGift();
    }
};

//送禮物
function sendGift() {
    var user = $('#nickname').val();
    var group = $('#group').val();
    var message = document.getElementById("info").innerHTML;
    var msg2grp = { "group": group, "name": user, "message": message,"data":data };
    var msgJSON = JSON.stringify(msg2grp);
    hubCon.server.sendGift(msgJSON).catch(function (err) {
        return console.error(err.toString());
    });
}

//接收禮物
hubCon.client.rcvGift = function (msgJSON) {
    var mJson = JSON.parse(msgJSON);

    var msgInfo = $('#info');
    msgInfo.html(msgInfo.html() + ";" + mJson.message);
    //$('body').css("background-image", "url('" + origin + "/Image/castle.png')").css('opacity', 1);

    //call app.js particlesJS(tag,func) for gift effect
    switch (mJson.data.trim()) {
        case '花':
            myPJS.particles.shape.image.src = "../assets/image/rose.png";
            myPJS.particles.move.speed = 6;
            break;
        case '煙火':
            myPJS.particles.shape.image.src = "../assets/image/fireworks.png";
            myPJS.particles.move.speed = 0.3;
            break;
        case '踩':
            myPJS.particles.shape.image.src = "../assets/image/sandals.png";
            myPJS.particles.size.value = 100;
            myPJS.particles.move.speed = 15;
            break;
        default:
            myPJS.particles.shape.image.src = "../assets/image/flowers.png";
            break;
    }
    $('#particles-js').css("background-image", "url('../Image/castle.png')");
    particlesJS('particles-js', myPJS);
};