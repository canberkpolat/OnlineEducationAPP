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

            var list = from s in unitOfWork.Streams.GetAll()
                     select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                list = list.Where(t => t.StreamName.Contains(searchString) || t.User.Name.Contains(searchString) || t.Course.Name.Contains(searchString));
            }
            else
            {
                return View(list.OrderByDescending(p => p.Id).Take(15).ToList());
            }
            //var streams = unitOfWork.Streams.GetAll().OrderByDescending(p => p.Id).Take(15).ToList();
            return View(list.ToList());
        }

        public async Task<IActionResult> Teachers()
        {
            var teachers = await userManager.GetUsersInRoleAsync("Teacher");
            return View(teachers);
        }
        public async Task<IActionResult> Students()
        {
            var students = await userManager.GetUsersInRoleAsync("Student");
            return View(students);
        }

        public IActionResult ActiveStreams()
        {
            var streams = unitOfWork.Streams.GetAll().Where(stream => stream.IsActive).ToList();
            return View(streams);
        }

        public IActionResult Course(int Id)
        {
            var streams = unitOfWork.Streams.GetAll().Where(stream => stream.CourseId == Id).ToList();
            return View(streams);
        }

        public IActionResult Category(int Id)
        {
            var streams = unitOfWork.Streams.GetAll().Where(stream => stream.Course.CategoryId == Id).ToList(); 
            return View("Course",streams);
        }



    }
}