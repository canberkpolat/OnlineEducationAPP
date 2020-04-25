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

        public StreamController(IUnitOfWork _unitOfWork, IStreamRepository _streamRepository)
        {
            streamRepository = _streamRepository;
            unitOfWork = _unitOfWork;
        }
        public IActionResult Create()
        {

            var courses = unitOfWork.Courses.GetAll().ToList();

            return View(courses);
        }
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public string Create(int courseId, string streamName,string userId)
        {
            var model = new Stream
            {
                CourseId = courseId,
                StartTime = null,
                IsActive = false,
                StreamName = streamName,
                UserId = userId,
                Endpoint  = "https://onlineeducationapp.canberkpolat.com/hls/",
                StreamKey = Guid.NewGuid().ToString()
            };
            streamRepository.Add(model);
            unitOfWork.SaveChanges();

            var streamLink = model.Endpoint + "/" + model.StreamKey;

            return streamLink;
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