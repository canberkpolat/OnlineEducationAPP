using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineEducationAPP.MvcWebUI.Repository.Abstract;

namespace OnlineEducationAPP.MvcWebUI.Controllers
{
    
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public DashboardController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;       
        }

        [Authorize(Roles ="Student")]
        public IActionResult Index()
        {
            
            return View();
        }
    }
}