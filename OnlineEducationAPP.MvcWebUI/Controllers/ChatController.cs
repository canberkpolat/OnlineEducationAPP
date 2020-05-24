using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OnlineEducationAPP.MvcWebUI.Hubs;

namespace OnlineEducationAPP.MvcWebUI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ChatController : Controller
    {
        private IHubContext<ChatHub> _chat;
        public ChatController(IHubContext<ChatHub> chat)
        {
            _chat = chat;
        }


        [HttpPost("[action]/{connectionId}/{roomName}")]
        public async Task<IActionResult> JoinRoom(string connectionId, string roomName)
        {
            await _chat.Groups.AddToGroupAsync(connectionId, roomName);
            return Ok();
        }
        //[HttpPost("[action]/{connectionId}/{roomName}")]
        //public async Task<IActionResult> LeaveRoom(string connectionId, string roomName)
        //{
        //    await _chat.Groups.RemoveFromGroupAsync(connectionId, roomName);
        //    return Ok();
        //}
        [HttpPost("[action]")]
        public async Task<IActionResult> SendMessage(string message,string roomName)
        {
            await _chat.Clients.Group(roomName).SendAsync("ReceiveMessage", User.Identity.Name, message);
            return Ok();
        }

        public IActionResult PrivateChat()
        {
            return View();
        }
    }
}