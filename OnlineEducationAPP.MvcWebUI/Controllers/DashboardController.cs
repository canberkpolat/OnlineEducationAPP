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
        private readonly IUnitOfWork unitOfWork;

        public DashboardController(UserManager<ApplicationUser> _userManager, IUnitOfWork _unitOfWork)
        {
            userManager = _userManager;
            unitOfWork = _unitOfWork;
        }

        public IActionResult Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var streamList = from stream in unitOfWork.Streams.GetAll().Include(t => t.User).Include(t => t.Course).ThenInclude(t=>t.Category)
                             select stream;

            if (String.IsNullOrEmpty(searchString))
            {
                return View(streamList.OrderByDescending(t => t.Id).Take(15).ToList());
            }
            streamList = streamList.Where(t => t.StreamName.Contains(searchString) || t.User.Name.Contains(searchString) || t.Course.Name.Contains(searchString));
            
            return View(streamList.ToList());
        }

        public async Task<IActionResult> Teachers()
        {
            var currentUserId = userManager.GetUserId(User);
            var teachers = await userManager.GetUsersInRoleAsync("Teacher");
            teachers = teachers.Where(t => t.Id != currentUserId).ToList();
            return View(teachers);
        }
        public async Task<IActionResult> Students()
        {
            var currentUserId = userManager.GetUserId(User);
            var students = await userManager.GetUsersInRoleAsync("Student");
            students = students.Where(t => t.Id != currentUserId).ToList();
            return View(students);
        }

        public IActionResult ActiveStreams()
        {
            var streams = unitOfWork.Streams.GetAll().Include(t => t.User).Include(t => t.Course).Where(stream => stream.IsActive).ToList();
            return View(streams);
        }

        public IActionResult Course(int Id)
        {
            var streams = unitOfWork.Streams.GetAll().Include(t => t.User).Include(t => t.Course).ThenInclude(t=>t.Category).Where(stream => stream.CourseId == Id).ToList();
            return View(streams);
        }

        public IActionResult Category(int Id)
        {
            var streams = unitOfWork.Streams.GetAll().Include(t => t.User).Include(t => t.Course).Where(stream => stream.Course.CategoryId == Id).ToList(); 
            return View("Course",streams);
        }


    }
}