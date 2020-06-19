using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using OnlineEducationAPP.MvcWebUI.Helpers.SignalRNotification;
using OnlineEducationAPP.MvcWebUI.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineEducationAPP.MvcWebUI.Hubs
{
    public class NotificationUserHub : Hub
    {
        private readonly IUserConnectionManager _userConnectionManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public NotificationUserHub(IUserConnectionManager userConnectionManager, UserManager<ApplicationUser> userManager)
        {
            _userConnectionManager = userConnectionManager;
            _userManager = userManager;
        }


        public string GetConnectionId()
        {
            var userId = _userManager.GetUserId(Context.User);
            _userConnectionManager.KeepUserConnection(userId, Context.ConnectionId);

            return Context.ConnectionId;
        }

        //Called when a connection with the hub is terminated.
        public async override Task OnDisconnectedAsync(Exception exception)
        {
            //get the connectionId
            var connectionId = Context.ConnectionId;
            _userConnectionManager.RemoveUserConnection(connectionId);
            var value = await Task.FromResult(0);
        }
    }
}
