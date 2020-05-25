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
using OnlineEducationAPP.MvcWebUI.Hubs;
using OnlineEducationAPP.MvcWebUI.Identity;
using OnlineEducationAPP.MvcWebUI.Repository.Abstract;

namespace OnlineEducationAPP.MvcWebUI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ChatController : Controller
    {
        private IHubContext<ChatHub> _chat;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public ChatController(IHubContext<ChatHub> chat, UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _chat = chat;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }


        [HttpPost("[action]")]
        public async Task<List<dynamic>> JoinRoom([FromBody] dynamic request)
        {
            await _chat.Groups.AddToGroupAsync((string)request.connectionId, (string)request.roomName.ToString());

            string[] tmp = ((string)request.roomName).Split("/");

            List<dynamic> response = new List<dynamic>();
            var messages = _unitOfWork.Messages.GetAll().Where(p => (p.SenderId == tmp[0] || p.ReceiverId == tmp[0]) && (p.SenderId == tmp[1] || p.ReceiverId == tmp[1])).OrderBy(p => p.SendTime).ToList();
            foreach(var message in messages)
            {
                if(message.ReceiveTime == null)
                {
                    message.ReceiveTime = DateTime.UtcNow;
                }
                response.Add(new { 
                    senderId = message.SenderId,
                    senderUserName = message.SenderUser.UserName,
                    imageTag = GravatarHtmlHelper.GetString(await GravatarHtmlHelper.GravatarImage(_userManager, message.SenderUser.Email, size: 48, defaultImage: GravatarHtmlHelper.DefaultImage.Identicon, rating: GravatarHtmlHelper.Rating.PG, cssClass: "")),
                    text = message.Messages
                });
            }
            _unitOfWork.SaveChanges();
            return response;
        }
        //[HttpPost("[action]/{connectionId}/{roomName}")]
        //public async Task<IActionResult> LeaveRoom(string connectionId, string roomName)
        //{
        //    await _chat.Groups.RemoveFromGroupAsync(connectionId, roomName);
        //    return Ok();
        //}
        [HttpPost("[action]")]
        public async Task<IActionResult> SendMessage([FromBody]dynamic request)
        {
            await _chat.Clients.Group((string)request.roomName).SendAsync("ReceiveMessage", User.Identity.Name, (string)request.message);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SendPrivateMessage([FromBody] dynamic request)
        {
            var user = await _userManager.GetUserAsync(User);
            var imageTag = await GravatarHtmlHelper.GravatarImage(_userManager, user.Email, size: 48, defaultImage: GravatarHtmlHelper.DefaultImage.Identicon, rating: GravatarHtmlHelper.Rating.PG, cssClass: "");

            var receiverUser = await _userManager.FindByIdAsync((string)request.receiverId);

            var msg = new Message
            {
                SenderUser = user,
                ReceiverUser = receiverUser,
                ReceiverId = (string)request.receiverId,
                SenderId = user.Id,
                Messages = (string)request.message,
                SendTime = DateTime.UtcNow,
            };
            _unitOfWork.Messages.Add(msg);
            
            await _chat.Clients.Group((string)request.roomName).SendAsync("ReceivePrivateMessage", User.Identity.Name, user.Id, GravatarHtmlHelper.GetString(imageTag), (string)request.message);
            _unitOfWork.SaveChanges();
            return Ok();
        }

        [Route("[action]/{receiverUserId}")]
        public async Task<IActionResult> PrivateChat(string receiverUserId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                 

                var roomId = "";
                if (receiverUserId.CompareTo(user.Id) == -1)
                {
                    roomId = user.Id + "/" + receiverUserId;
                }
                else
                {
                    roomId = receiverUserId + "/" + user.Id;
                }

                ViewBag.RoomId = roomId;
                ViewBag.UserId = user.Id;
                ViewBag.ReceiverUserId = receiverUserId;
                return View();
            }
            catch (Exception ex)
            {

                throw;
            }
         

            
        }
    }
}