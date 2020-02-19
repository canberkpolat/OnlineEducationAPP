using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineEducationAPP.MvcWebUI.Controllers
{
    
    public class DashboardController : Controller
    {
        [Authorize(Roles ="Student")]
        public IActionResult Index()
        {
            return View();
        }
    }
}