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
    [Authorize(Roles = "Teacher")]
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
        [HttpPost]
        public  IActionResult Create(int courseId, string streamName,string userId)
        {
            

            var model = new Stream
            {
                CourseId = courseId,
                StartTime = null,
                IsActive = true,
                StreamName = streamName,
                UserId = userId,
                Endpoint  ="test_endpoint",
                StreamKey ="test_key"
            };
            streamRepository.Add(model);
            unitOfWork.SaveChanges();

            var streamLink = model.Endpoint + "/" + model.StreamKey;

            return RedirectToAction("Create", model);
        }

        [HttpGet]
        [Route("Stream/Start/{key}")]
        public void Start(string key)
        {
            var stream = streamRepository.Find(p => p.StreamKey == key).FirstOrDefault();
            stream.IsActive = true;
            stream.StartTime = DateTime.Now;

            unitOfWork.SaveChanges();
        }

        [HttpGet]
        [Route("Stream/End/{key}")]
        public void End(string key)
        {
            var stream = streamRepository.Find(p => p.StreamKey == key).FirstOrDefault();
            stream.IsActive = false;
            stream.EndTime = DateTime.Now;

            unitOfWork.SaveChanges();
        }
    }
}