using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using OnlineEducationAPP.MvcWebUI.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace OnlineEducationAPP.MvcWebUI.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
 
        public ChatHub(UserManager<ApplicationUser> _userManager, IHttpContextAccessor httpContextAccessor)
        {
            userManager = _userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task SendMessage(string user,string message)
        {
            bool isCurrentUser = true;
            if (_httpContextAccessor.HttpContext.User.Identity.Name != user)
                isCurrentUser = false;
            var currentUser = await userManager.FindByNameAsync(user);
            var imgUrl = currentUser.ProfileImageUrl;
            var date = DateTime.Now.ToLongTimeString();
            await Clients.All.SendAsync("ReceiveMessage", imgUrl,date, message, isCurrentUser);
        }
    }
}
