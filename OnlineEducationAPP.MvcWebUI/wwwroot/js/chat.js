"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (imgUrl, date, message, isCurrentUser) {
        var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
        var encodedMsg = msg;

    //    var elm = document.getElementById('messageList');
    

    //    var msg_time_send = document.createElement('span');
    //    msg_time_send.className = "msg_time_send";
    //    msg_time_send.appendChild(document.createTextNode(date));

    //    var msg_cotainer_send = document.createElement('div')
    //    msg_cotainer_send.className = "msg_cotainer_send";
    //    msg_cotainer_send.appendChild(document.createTextNode(encodedMsg));
    //    msg_cotainer_send.append(msg_time_send);

    //    var innerImg = document.createElement('img');
    //    innerImg.src = imgUrl;
    //    innerImg.className = "rounded-circle user_img_msg"

    //    var img_cont_msg = document.createElement('div');
    //    img_cont_msg.className = "img_cont_msg";
    //    img_cont_msg.append(innerImg);


    //    var parentClass = document.createElement('div');
    //    parentClass.className = "d-flex justify-content-end mb-4";
    //    parentClass.append(msg_cotainer_send, img_cont_msg);

    //    var li = document.getElementById('li');
    //    li.append(parentClass);

    //elm.append(li);

    var elm = document.getElementById('messageList');

  

    var p = document.createElement('p');
    p.appendChild(document.createTextNode(encodedMsg));

    var chatContent = document.createElement('div');
    chatContent.className = "chat-content";
    chatContent.append(p);

    var chatBody = document.createElement('div');
    chatBody.className = "chat-body";
    chatBody.append(chatContent);

    var innerImg = document.createElement('img');
    innerImg.src = imgUrl;
    innerImg.alt = "avatar";

    var avatar = document.createElement('a');
    avatar.className = "avatar";
    avatar.append(innerImg);

    var chatAvatar = document.createElement('div');
    chatAvatar.className = "chat-avatar";
    chatAvatar.append(avatar);

    var chat = document.getElementById('chat');
    chat.append(chatAvatar, chatBody);

    var li = document.getElementById('li');
    li.append(chat);

    elm.append(li);
    
   

});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});