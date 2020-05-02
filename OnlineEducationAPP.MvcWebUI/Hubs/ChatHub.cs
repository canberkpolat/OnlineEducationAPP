using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using OnlineEducationAPP.MvcWebUI.Identity;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace OnlineEducationAPP.MvcWebUI.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        //private readonly UserManager<ApplicationUser> userManager;
 
        //public ChatHub(UserManager<ApplicationUser> _userManager)
        //{
        //    userManager = _userManager;
        //}

        public string GetConnectionId() => Context.ConnectionId;


        //public async Task SendMessage(string message)
        //{
        //    await Clients.All.SendAsync("ReceiveMessage", Context.User.Identity.Name, message);
        //}
    }
}
