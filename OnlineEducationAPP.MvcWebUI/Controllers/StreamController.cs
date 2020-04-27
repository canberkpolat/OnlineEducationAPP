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

    public class StreamController : Controller
    {
        private IStreamRepository streamRepository;
        private IUnitOfWork unitOfWork;
        private UserManager<ApplicationUser> userManager;

        public StreamController(IUnitOfWork _unitOfWork, IStreamRepository _streamRepository, UserManager<ApplicationUser> _userManager)
        {
            streamRepository = _streamRepository;
            unitOfWork = _unitOfWork;
            userManager = _userManager;
        }
        public IActionResult Create()
        {
         
            var courses = unitOfWork.Courses.GetAll().ToList();

            return View(courses);
        }
        [Authorize(Roles = "Teacher")]
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
                LiveStreamEndpoint = "https://onlineeducationapp.canberkpolat.com:8443/hls/"+ streamKey,
                VideoOnDemandEndpoint = "rtmp://onlineeducationapp.canberkpolat.com:8080/vod/"+ streamKey+".flv",
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