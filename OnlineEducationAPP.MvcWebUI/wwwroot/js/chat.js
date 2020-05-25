"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
var _connectionId = '';
var roomName = $('#room-name').val();
//Disable send button until connection is established
$("#sendButton").disabled = true;

connection.on("ReceiveMessage", function (username, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");

    var msgList = $('#chats');

    var backgroundColor = intToRGB(hashCode(username));
    var textColor = invertColor(backgroundColor);


    msgList.append('\
        <div class="chat chat-left">\
            <div class="chat-body">\
                <div class="chat-content" style="color: '+ textColor + '; background-color: ' + backgroundColor + ';">' + username + ': ' + msg + '</div>\
            </div>\
        </div>');
});


connection.on("ReceivePrivateMessage", function (username, senderId, imageTag,message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var msgList = $('#chats');

    var backgroundColor = intToRGB(hashCode(username));
    var textColor = invertColor(backgroundColor);
    var align = "";
    var placement = "right"
    if (senderId != userId) {

        align = "chat-left";
        placement = "left";
    } 
    msgList.append('\
        <div class="chat ' + align +'">\
         <div class="chat-avatar">\
            <a class="avatar" data-toggle="tooltip" data-placement="'+placement+'" >\
           ' + imageTag + '\
            </a>\
        </div>\
            <div class="chat-body">\
                <div class="chat-content" style="color: '+ textColor + '; background-color: ' + backgroundColor + ';">' + username + ': ' + msg + '</div>\
            </div>\
        </div>');
});


var joinRoom = function () {
    var url = '/Chat/JoinRoom';
    var body = { "connectionId": _connectionId, "roomName": roomName };
    axios.post(url, body)
        .then(res => {
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
    $("#sendButtonPrivate").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});


var input = document.getElementById("messageInput");
input.addEventListener("keyup", function (event) {
    if (event.keyCode === 13) {
        event.preventDefault();
        document.getElementById("sendButton").click();
    }
});


$("#sendButton").on("click", function () {

    var message = $("#messageInput").val();
    $("#messageInput").val('');
    axios.post('/Chat/SendMessage', { "message": message, "roomName": roomName})
        .then(res => {
            console.log("Message Sent", res);
        })
        .catch(err => {
            console.log("Failed to Sent Messages", err);
        })

    //connection.invoke("SendMessage", message).catch(function (err) {
    //    return console.error(err.toString());
    //});
    event.preventDefault();
});

$("#sendButtonPrivate").on("click", function () {

    var message = $("#messageInput").val();
    $("#messageInput").val('');
    axios.post('/Chat/SendPrivateMessage', { "message": message, "roomName": roomName, "receiverId": receiverId })
        .then(res => {
            console.log("Message Sent", res);
        })
        .catch(err => {
            console.log("Failed to Sent Messages", err);
        })

    //connection.invoke("SendMessage", message).catch(function (err) {
    //    return console.error(err.toString());
    //});
    event.preventDefault();
});


function hashCode(str) { // java String#hashCode
    var hash = 0;
    for (var i = 0; i < str.length; i++) {
        hash = str.charCodeAt(i) + ((hash << 5) - hash);
    }
    return hash;
}

function intToRGB(i) {
    var c = (i & 0x00FFFFFF)
        .toString(16)
        .toUpperCase();

    return "#" + ("00000".substring(0, 6 - c.length) + c);
}

function invertColor(hex) {
    if (hex.indexOf('#') === 0) {
        hex = hex.slice(1);
    }
    // convert 3-digit hex to 6-digits.
    if (hex.length === 3) {
        hex = hex[0] + hex[0] + hex[1] + hex[1] + hex[2] + hex[2];
    }
    if (hex.length !== 6) {
        throw new Error('Invalid HEX color.');
    }
    var r = parseInt(hex.slice(0, 2), 16),
        g = parseInt(hex.slice(2, 4), 16),
        b = parseInt(hex.slice(4, 6), 16);
    return (r * 0.299 + g * 0.587 + b * 0.114) > 186
        ? '#000000'
        : '#FFFFFF';
}
