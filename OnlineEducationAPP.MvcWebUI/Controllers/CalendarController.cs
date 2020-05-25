using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineEducationAPP.MvcWebUI.Entity;
using OnlineEducationAPP.MvcWebUI.Identity;
using OnlineEducationAPP.MvcWebUI.Repository.Abstract;

namespace OnlineEducationAPP.MvcWebUI.Controllers
{
    public class CalendarController : Controller
    {
        private IUnitOfWork unitOfWork;
        private UserManager<ApplicationUser> userManager;
        
        public CalendarController(IUnitOfWork _unitOfWork, UserManager<ApplicationUser> _userManager)
        {
            unitOfWork = _unitOfWork;
            userManager = _userManager;
        }
        [Authorize(Roles = "Teacher,Admin")]
        [Route("Calendar/Event/Create")]
        [HttpPost]
        public void Create([FromBody] Event a)
        {
            unitOfWork.Events.Add(a);
            unitOfWork.SaveChanges();
        }

        [Authorize(Roles = "Student,Teacher,Admin")]
        [Route("Calendar/Event/List")]
        [HttpGet]
        public IActionResult List(int Id)
        {
            var events = unitOfWork.Events.GetAll().ToList();
            return View(events);
        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpPost]
        public dynamic Create(int courseId, string streamName)
        {
            var userID = userManager.GetUserId(User);

            string streamKey = Guid.NewGuid().ToString();

            var model = new Stream
            {
                CourseId = courseId,
                StartTime = null,
                IsActive = false,
                StreamName = streamName,
                UserId = userID,
                LiveStreamEndpoint = "https://onlineeducationapp.canberkpolat.com:8443/live/",
                VideoOnDemandEndpoint = "https://onlineeducationapp.canberkpolat.com:8443/vod/",
                StreamKey = streamKey
            };
            streamRepository.Add(model);
            unitOfWork.SaveChanges();

            var response = new
            {
                StreamEndPoint = "rmtp://onlineeducationapp.canberkpolat.com:8080/live",
                StreamKey = streamKey
            };

            return response;
        }

        [HttpPost]
        [Route("Stream/Start")]
        public void Start([FromForm] string name)
        {
            var stream = streamRepository.Find(p => p.StreamKey == name && p.EndTime == null).FirstOrDefault();
            if(stream != null) { 
                stream.IsActive = true;
                stream.StartTime = DateTime.Now;

                unitOfWork.SaveChanges();
            }
            else
            {
                throw new Exception("Invalid stream key.");
            }
        }

        [HttpPost]
        [Route("Stream/Update")]
        public void Update([FromForm] string name)
        {
            var stream = streamRepository.Find(p => p.StreamKey == name && p.EndTime == null).FirstOrDefault();
            if (stream != null)
            {
                stream.IsActive = true;
                stream.UpdateTime = DateTime.Now;

                unitOfWork.SaveChanges();
            }
            else
            {
                throw new Exception("Invalid stream key.");
            }
        }

        [HttpPost]
        [Route("Stream/End")]
        public void End([FromForm] string name)
        {
            var stream = streamRepository.Find(p => p.StreamKey == name).FirstOrDefault();
            if (stream != null)
            {
                stream.IsActive = false;
                stream.EndTime = DateTime.Now;

                unitOfWork.SaveChanges();
            }
            else
            {
                throw new Exception("Invalid stream key.");
            }
        }
    }
}