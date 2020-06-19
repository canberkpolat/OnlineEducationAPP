using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using OnlineEducationAPP.MvcWebUI.Entity;
using OnlineEducationAPP.MvcWebUI.Helpers.SignalRNotification;
using OnlineEducationAPP.MvcWebUI.Hubs;
using OnlineEducationAPP.MvcWebUI.Identity;
using OnlineEducationAPP.MvcWebUI.Repository.Abstract;

namespace OnlineEducationAPP.MvcWebUI.Controllers
{
    //[Authorize]
    //[Route("[controller]")]
    public class ChatController : Controller
    {
        private IHubContext<ChatHub> _chat;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<NotificationUserHub> _notificationUserHubContext;
        private readonly IUserConnectionManager _userConnectionManager;
        public ChatController(IHubContext<ChatHub> chat, UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IHubContext<NotificationUserHub> notificationUserHubContext, IUserConnectionManager userConnectionManager)
        {
            _chat = chat;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _notificationUserHubContext = notificationUserHubContext;
            _userConnectionManager = userConnectionManager;
        }


        [HttpPost("[controller]/[action]")]
        public async Task<List<dynamic>> JoinRoom([FromBody] dynamic request)
        {
            string RoomName = (string)request.roomName;
            await _chat.Groups.AddToGroupAsync((string)request.connectionId, RoomName);
            List<dynamic> response = new List<dynamic>();
            //var messages = _unitOfWork.Messages.GetAll().Where(p => p.RoomName == RoomName).OrderBy(p => p.SendTime).ToList();
            var messages = _unitOfWork.Messages.Find(t => t.RoomName == RoomName).Include(t => t.SenderUser).OrderBy(t => t.SendTime).ToList();
            foreach (var message in messages)
            {
                if (message.ReceiveTime == null)
                {
                    message.ReceiveTime = DateTime.UtcNow;
                }
                response.Add(new
                {
                    senderId = message.SenderId,
                    senderUserName = message.SenderUser.UserName,
                    imageTag = GravatarHtmlHelper.GetString(await GravatarHtmlHelper.GravatarImage(_userManager, message.SenderUser.Email, size: 48, defaultImage: GravatarHtmlHelper.DefaultImage.Identicon, rating: GravatarHtmlHelper.Rating.PG, cssClass: "")),
                    text = message.Messages
                });
            }
            _unitOfWork.SaveChanges();

            return response;
        }

        [HttpPost("[controller]/[action]")]
        public async Task<IActionResult> SendMessage([FromBody] dynamic request)
        {
            var user = await _userManager.GetUserAsync(User);
            Guid userId = Guid.Parse(user.Id);
            string roomName = (string)request.roomName;
            string receiverUserId = null;
            if (roomName.Contains("/"))
            {
                string[] tmp = roomName.Split("/");
                Guid user1, user2;
                if (Guid.TryParse(tmp[0], out user1) && Guid.TryParse(tmp[1], out user2))
                {
                    if (userId.Equals(user1))
                    {
                        receiverUserId = user2.ToString();
                    }
                    else if (userId.Equals(user2))
                    {
                        receiverUserId = user1.ToString();
                    }
                    else
                    {
                        throw new Exception("Not authorized!");
                    }
                }
            }
            string message = (string)request.message;

            var imageTag = await GravatarHtmlHelper.GravatarImage(_userManager, user.Email, size: 48, defaultImage: GravatarHtmlHelper.DefaultImage.Identicon, rating: GravatarHtmlHelper.Rating.PG, cssClass: "");

            var msg = new Message
            {
                ReceiverId = receiverUserId,
                SenderId = user.Id,
                Messages = message,
                SendTime = DateTime.UtcNow,
                RoomName = roomName
            };
            _unitOfWork.Messages.Add(msg);

            await _chat.Clients.Group(roomName).SendAsync("ReceiveMessage", User.Identity.Name, user.Id, GravatarHtmlHelper.GetString(imageTag), message);
            _unitOfWork.SaveChanges();

            string receiverId = (string)request.receiverId;

            //var messages = _unitOfWork.Messages.Find(t => t.ReceiverId == receiverId && t.ReceiveTime == null)
            //                                   .OrderByDescending(t => t.SendTime)
            //                                   .GroupBy(t => t.SenderUser)
            //                                   .ToList();
            //if (messages.Count == 0 || messages == null)
            //{
            //    return Json("Notification couldnt sent because There is no notification");
            //}

            var connections = _userConnectionManager.GetUserConnections(receiverId);
            if (connections != null && connections.Count > 0)
            {
                foreach (var connectionId in connections)
                {
                    
                        await _notificationUserHubContext.Clients.Client(connectionId).SendAsync("sendToUser", receiverId);
                }
            }


            return Ok();
        }


        [HttpPost]
        [Route("[controller]/[action]")]
        public void ReadMessage([FromBody] dynamic obj)
        {
            var currentUserId = _userManager.GetUserId(User);
            var senderId = (string)obj.senderId;
            var message = _unitOfWork.Messages.Find(t => t.ReceiverId == currentUserId && t.SenderId == senderId).ToList();
            var now = DateTime.Now;
            message.ForEach(t => t.ReceiveTime = now);

            _unitOfWork.SaveChanges();
        }


        [Route("[controller]/[action]/{receiverUserId}")]
        public async Task<IActionResult> PrivateChat(string receiverUserId)
        {
            var user = await _userManager.GetUserAsync(User);

            ViewBag.RoomId = GetRoomName(user.Id, receiverUserId);
            ViewBag.ReceiverId = receiverUserId;
            ViewBag.UserId = user.Id;
            return View();
        }

        public string GetRoomName(string xid, string yid)
        {
            var roomName = "";
            if (xid.CompareTo(yid) == -1)
            {
                roomName = yid + "/" + xid;
            }
            else
            {
                roomName = xid + "/" + yid;
            }
            return roomName;
        }

        public List<dynamic> GetUnreadedMessages(string id)
        {
            var response = new List<dynamic>();

            var unreadedMessages = _unitOfWork.Messages.Find(t => t.ReceiverId == id && t.ReceiveTime == null).Include(t => t.SenderUser)
                                               .OrderByDescending(t => t.SendTime)
                                               .ToList();

            unreadedMessages.ForEach(t => response.Add(new
            {
                senderName = t.SenderUser.Name,
                senderImage = t.SenderUser.ProfileImageUrl,
                message = t.Messages,
                senderId = t.SenderId

            }));

            return response;
        }

    }
}