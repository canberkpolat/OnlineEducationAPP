﻿using System;
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
        private IUnitOfWork unitOfWork;
        private UserManager<ApplicationUser> _userManager;
        
        public StreamController(IUnitOfWork _unitOfWork, UserManager<ApplicationUser> userManager)
        {
            unitOfWork = _unitOfWork;
            _userManager = userManager;
        }
        [Authorize(Roles = "Teacher,Admin")]
        [Route("Stream/Create")]
        [HttpGet]
        public IActionResult Create()
        {

            var courses = unitOfWork.Courses.GetAll().ToList();
            return View(courses);
        }

        [Authorize(Roles = "Student,Teacher,Admin")]
        [Route("Stream/{Id}")]
        [HttpGet]
        public IActionResult Stream(int Id)
        {
            var stream = unitOfWork.Streams.Get(Id);
            stream.AmaountShown++;
            unitOfWork.SaveChanges();
            ViewBag.UserId = _userManager.GetUserId(User);
            return View(stream);
        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpPost]
        public dynamic Create(int courseId, string streamName)
        {
            var userID = _userManager.GetUserId(User);

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
            unitOfWork.Streams.Add(model);
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
            var stream = unitOfWork.Streams.Find(p => p.StreamKey == name && p.EndTime == null).FirstOrDefault();
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
            var stream = unitOfWork.Streams.Find(p => p.StreamKey == name && p.EndTime == null).FirstOrDefault();
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
            var stream = unitOfWork.Streams.Find(p => p.StreamKey == name).FirstOrDefault();
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