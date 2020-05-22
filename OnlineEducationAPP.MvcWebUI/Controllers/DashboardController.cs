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
using Microsoft.EntityFrameworkCore;

namespace OnlineEducationAPP.MvcWebUI.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IStreamRepository streamRepository;
        public DashboardController(UserManager<ApplicationUser> _userManager, IStreamRepository _streamRepository)
        {
            streamRepository = _streamRepository;
            userManager = _userManager;
        }

        public IActionResult Index()
        {
            var streams = streamRepository.GetAll().OrderByDescending(p => p.Id).Take(15).ToList();
            return View(streams);
        }

        public IActionResult UserCards()
        {
            var users = userManager.Users.ToList();
            return View(users);
        }

        public IActionResult ActiveStreams()
        {
            var streams = streamRepository.GetAll().Where(stream => stream.IsActive).ToList();
            return View(streams);
        }

        public IActionResult Course(int Id)
        {
            var streams = streamRepository.GetAll().Where(stream => stream.CourseId == Id).ToList();
            return View(streams);
        }

        public IActionResult Category(int Id)
        {
            var streams = streamRepository.GetAll().Where(stream => stream.Course.CategoryId == Id).ToList(); 
            return View("Course",streams);
        }
    }
}