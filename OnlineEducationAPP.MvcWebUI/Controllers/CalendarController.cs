using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using OnlineEducationAPP.MvcWebUI.Entity;
using OnlineEducationAPP.MvcWebUI.Hubs;
using OnlineEducationAPP.MvcWebUI.Identity;
using OnlineEducationAPP.MvcWebUI.Repository.Abstract;

namespace OnlineEducationAPP.MvcWebUI.Controllers
{
    public class CalendarController : Controller
    {
        private IUnitOfWork unitOfWork;
        private UserManager<ApplicationUser> userManager;
        private readonly IHubContext<NotificationUserHub> notificationUserHubContext;


        public CalendarController(IUnitOfWork _unitOfWork, UserManager<ApplicationUser> _userManager, IHubContext<NotificationUserHub> _notificationUserHubContext)
        {
            unitOfWork = _unitOfWork;
            userManager = _userManager;
            notificationUserHubContext = _notificationUserHubContext;
        }

        public IActionResult Index()
        {
            return View();
        }


        public List<dynamic> GetEvents()
        {
            var events = unitOfWork.Events.GetAll().Include(t=>t.User).Include(t => t.Course).ToList();

            var response = new List<dynamic>();

            foreach (var e in events)
            {
                response.Add(new
                {
                    Id = e.Id,
                    Text = e.Text,
                    Description = e.Description,
                    Start = e.DateBegin,
                    End = e.DateEnd,
                    Name = e.User.Name,
                    SurName = e.User.Surname,
                    Course = e.Course.Name,
                    CourseId = e.CourseId
                });
            }

            return response;
        }

        public async Task<IActionResult> SaveEvent(Event e)
        {
            var user = await userManager.GetUserAsync(User);
            e.UserId = user.Id;

            if (e.Id > 0)
            {
                var eventt = unitOfWork.Events.Get(e.Id);
                if (eventt != null)
                {
                    eventt.Text = e.Text;
                    eventt.Description = e.Description;
                    eventt.DateBegin = e.DateBegin;
                    eventt.DateEnd = e.DateEnd;
                    eventt.UserId = user.Id;
                }
            }
            else
            {
                unitOfWork.Events.Add(e);
            }

            unitOfWork.SaveChanges();

            await notificationUserHubContext.Clients.All.SendAsync("sendCalendarNotification", user.Name, user.Surname, e.Description);

            return Ok();
        }

        [HttpPost]
        public IActionResult DeleteEvent(int eventId)
        {

            var eventt = unitOfWork.Events.Get(eventId);
            if (eventt != null)
            {
                unitOfWork.Events.Delete(eventt);
                unitOfWork.SaveChanges();
            }
            return Ok();
        }

        public async Task<IActionResult> IsTeacherOrAdmin()
        {
            var currentUser = await userManager.GetUserAsync(User);

            var roles = await userManager.GetRolesAsync(currentUser);

            if (roles.Contains("Teacher") || roles.Contains("Admin"))
            {
                return Ok();
            }

            throw new Exception();
        }

        public async Task<IActionResult> IsStudent()
        {
            var currentUser = await userManager.GetUserAsync(User);

            var roles = await userManager.GetRolesAsync(currentUser);

            if (roles.Contains("Student"))
                return Ok();

            throw new Exception();
        }


        public List<dynamic> GetCourseList()
        {
            var response = new List<dynamic>();
            var courses = unitOfWork.Courses.GetAll().ToList();

            foreach (var course in courses)
            {
                response.Add(new
                {
                    courseId = course.Id,
                    courseName = course.Name
                });
            };

            return response;
        }

    }
}