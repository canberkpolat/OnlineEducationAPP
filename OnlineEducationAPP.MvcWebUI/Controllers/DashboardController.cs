using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineEducationAPP.MvcWebUI.Identity;
using OnlineEducationAPP.MvcWebUI.Repository.Abstract;

namespace OnlineEducationAPP.MvcWebUI.Controllers
{
    
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        public DashboardController(UserManager<ApplicationUser> _userManager)
        {
            userManager = _userManager;
        }

        [Authorize(Roles = "Student")]
        public IActionResult Index()
        {
            
            return View();
        }

        public IActionResult UserCards()
        {
            var users = userManager.Users.ToList();
            return View(users);
        }

    }
}