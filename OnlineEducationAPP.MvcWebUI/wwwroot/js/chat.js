"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
var _connectionId = '';
var roomName = $('#room-name').val();
var receiverUserId = $('#receiver-user-id').val();
//Disable send button until connection is established
$("#sendButton").disabled = true;

if (typeof roomName !== 'undefined') {
    connection.on("ReceiveMessage", function (username, senderId, imageTag, message) {
        addChatMessage(senderId, username, imageTag, message);
        axios.post('/Chat/ReadMessage', { "senderId": senderId })
            .then(res => {
                console.log("Readed", res);
            })
            .catch(err => {
                console.log("Not Readed", err);
            })

    });

    var joinRoom = function () {
        var url = '/Chat/JoinRoom';
        var body = { "connectionId": _connectionId, "roomName": roomName };
        axios.post(url, body)
            .then(res => {
                for (var i = 0; i < res.data.length; i++) {
                    var obj = res.data[i];
                    addChatMessage(obj.senderId, obj.senderUserName, obj.imageTag, obj.text);
                }
                $('#loading').hide();
                console.log("Room Joined", res);
            })
            .catch(err => {
                console.error("Failed to join Room", err);
            })
    }

    connection.start().then(function () {
        connection.invoke('getConnectionId').then(function (connectionId) {
            _connectionId = connectionId;
            joinRoom();
        })
        $("#sendButton").disabled = false;
    }).catch(function (err) {
        return console.error(err.toString());
    });


    var input = $('#messageInput');
    input.keyup(function (event) {
        if (event.keyCode === 13) {
            event.preventDefault();
            $('#sendButton').trigger("click");
            $('#sendButtonPrivate').trigger("click");
        }
    });

    function SendMessage() {
        var message = $("#messageInput").val();
        $("#messageInput").val('');
        axios.post('/Chat/SendMessage', { "message": message, "roomName": roomName, "receiverId": receiverUserId})
            .then(res => {
                console.log("Message Sent", res);
            })
            .catch(err => {
                console.log("Failed to Sent Messages", err);
            })
    }
    //function SendNotification() {
    //    var url = '/Chat/SendToSpecificUser';
    //    var body = { "receiverId": receiverUserId };
    //    axios.post(url, body)
    //        .then(res => {
    //            console.log("Notification Sent", res);
    //        })
    //        .catch(err => {
    //            console.log("Failed to Sent Notification", err);
    //        })
    //}

    $("#sendButton").on("click", function () {
        SendMessage();
        //SendNotification();

        event.preventDefault();
    });
}

function addChatMessage(senderId, username, imageTag, msg) {
   
    if (typeof $('#player').val() !== 'undefined') {
        $(".chat-application").css("height", $('#player').css("height"));
    }
    var placement = "right";
    if (senderId !== userId || typeof $('#player').val() !== 'undefined') {
        placement = "left";
    }
    msg = msg.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var msgList = $('#chats');
    var backgroundColor = intToRGB(hashCode(username));
    var textColor = invertColor(backgroundColor);
    var align = "chat-" + placement;
    msgList.append('\
        <div class="chat ' + align + '">\
            <div class="chat-avatar">\
            <a class="avatar" data-toggle="tooltip" data-placement="'+ placement + '" >\
            ' + imageTag + '\
            </a>\
        </div>\
            <div class="chat-body">\
                <div class="chat-content" style="color: '+ textColor + '; background-color: ' + backgroundColor + ';">' + username + ': ' + msg + '</div>\
            </div>\
        </div>');
    $('.chat-app-window').scrollTop($('.chat-app-window').height()*10);
}