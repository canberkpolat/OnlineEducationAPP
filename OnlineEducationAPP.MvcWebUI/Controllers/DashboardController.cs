using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineEducationAPP.MvcWebUI.Identity;
using OnlineEducationAPP.MvcWebUI.Models;
using OnlineEducationAPP.MvcWebUI.Repository.Abstract;

namespace OnlineEducationAPP.MvcWebUI.Controllers
{
    
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IStreamRepository streamRepository;
        public DashboardController(UserManager<ApplicationUser> _userManager, IStreamRepository _streamRepository)
        {
            streamRepository = _streamRepository;
            userManager = _userManager;
        }

        [Authorize(Roles = "Student, Teacher")]
        public IActionResult Index()
        {
            
            return View();
        }

        public IActionResult UserCards()
        {
            var users = userManager.Users.ToList();
            return View(users);
        }
          
        public IActionResult ActiveStreams()
        {
            var streams = streamRepository.GetAll().Where(stream => stream.IsActive).ToList();
            var users = userManager.Users.ToList();
            var model = new ActiveStreamViewModel
            {
                Streams = streams,
                Users = users
            };
            return View(model);
        }

    }
}