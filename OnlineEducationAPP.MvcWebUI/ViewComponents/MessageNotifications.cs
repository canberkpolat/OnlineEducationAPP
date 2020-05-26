using Microsoft.AspNetCore.Mvc;
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

        public MessageNotifications(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }


        public IViewComponentResult Invoke()
        {
            var messages = unitOfWork.Messages.Find(t => t.ReceiveTime == null).OrderByDescending(t=>t.SendTime).GroupBy(t => t.SenderUser).ToList();

            return View(messages);
        }
    }
}
