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
                StartTime = DateTime.Now,
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
        
    }
}