using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineEducationAPP.MvcWebUI.Identity;
using OnlineEducationAPP.MvcWebUI.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineEducationAPP.MvcWebUI.ViewComponents
{
    public class MessageNotifications : ViewComponent
    {

        private IUnitOfWork unitOfWork;
        private UserManager<ApplicationUser> userManager;

        public MessageNotifications(IUnitOfWork _unitOfWork, UserManager<ApplicationUser> _userManager)
        {
            unitOfWork = _unitOfWork;
            userManager = _userManager;
        }


        public IViewComponentResult Invoke()
        {
            var currentUserName = User.Identity.Name;
            var messages = unitOfWork.Messages.Find(t => t.ReceiveTime == null && t.ReceiverUser.UserName == currentUserName)
                                              .OrderByDescending(t=>t.SendTime)
                                              .GroupBy(t => t.SenderUser)
                                              .ToList();

            return View(messages);
        }
    }
}
