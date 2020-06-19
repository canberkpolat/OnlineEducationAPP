"use strict";
//var currentUserId = $('#current-user-id').val();
var receiverUserId = $('#receiver-user-id').val();
var con = new signalR.HubConnectionBuilder().withUrl("/NotificationUserHub").build();
var isStudent = false;
var notificationCount = 0;
IsStudent();

function AddNotification(msgs) {
    console.log('message length' , msgs)
    $('#notificationList').empty();
    if (msgs.length != 0) {
        $('#outerNotify').text(msgs.length);
        $('#innerNotify span').text(msgs.length + ' New');
    }
    msgs.forEach(message => {
        $('#notificationList').append('\
        <a href = "/Chat/PrivateChat/'+ message.senderId + '">  \
            <div class="media">\
                <div class="media-left">\
                   <span class="avatar rounded-circle">\
                        <img src="'+ message.senderImage + '" alt="avatar"><i></i>\
                   </span>\
                </div>\
                <div class="media-body">\
                     <h6 class="media-heading">'+ message.senderName + '</h6>\
                     <p class="notification-text font-small-3 text-muted">'+ message.message + '</p>\
                </div>\
            </div>\
        </a>');
    });
}

function IsStudent() {
    $.ajax({
        type: "GET",
        url: '/Calendar/IsStudent',
        success: function () {
            isStudent = true;
            console.log(isStudent);
        },
        error: function () {
            isStudent = false;
            console.log(isStudent);
        }
    });
}

con.on("sendToUser", (id) => {
    $.ajax({
        type: "GET",
        url: "/Chat/GetUnreadedMessages",
        data: { "id": id },
        success: function (data) {
            AddNotification(data);
        },
        error: function (hata) {
            alert(hata.status);
            alert(hata.statusCode);
        }
    });

});


con.on("sendCalendarNotification", function (name, surname, description) {
    if (isStudent == true) {
        notificationCount++;
        $('#innerNoti').text(notificationCount + ' New');
        $('#outerNoti').text(notificationCount);
            $('#calendarNotification').append('\
            <a href = "/Calendar/Index" >\
                <div class= "media" >\
                    <div class="media-left align-self-center"><i class="ft-plus-square icon-bg-circle bg-cyan"></i></div>\
                    <div class="media-body">\
                        <h6 class="media-heading">'+ name +' is added a new class</h6>\
                        <p class="notification-text font-small-3 text-muted">'+ description + '</p>\
                        <small>\
                            <time class="media-meta text-muted">Just Now</time>\
                        </small>\
                     </div>\
                 </div >\
            </a >');


    }
});


//$("#sendButton").click(function () {
//    $("#messageInput").val('');
//    var url = '/Chat/SendToSpecificUser';
//    var body = { "receiverId": receiverUserId };
//    axios.post(url, body)
//        .then(res => {
//            console.log("Notification Sent", res);
//        })
//        .catch(err => {
//            console.log("Failed to Sent Notification", err);
//        })
//    event.preventDefault();
//});


con.start().catch(function (err) {

    return console.error(err.toString());
}).then(function () {
    con.invoke('GetConnectionId').then(function (connectionId) {
        _connectionId = connectionId;
    }).then(res => {
        console.log(_connectionId);
    })
});

