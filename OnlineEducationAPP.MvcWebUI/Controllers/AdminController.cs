using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineEducationAPP.MvcWebUI.Entity;
using OnlineEducationAPP.MvcWebUI.Identity;
using OnlineEducationAPP.MvcWebUI.Models;
using OnlineEducationAPP.MvcWebUI.Repository.Abstract;

namespace OnlineEducationAPP.MvcWebUI.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(IUnitOfWork unitOfWork
            ,IHostingEnvironment hostingEnvironment
            ,UserManager<ApplicationUser> userManager
            , RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult GetCourses()
        {
            var courses = _unitOfWork.Courses.GetAll().Include(t=>t.Category).ToList();

            return View(courses);
        }
        [HttpPost]
        public IActionResult DeleteCourseById(int id)
        {
            var course = _unitOfWork.Courses.Get(id);
            _unitOfWork.Courses.Delete(course);
            _unitOfWork.SaveChanges();

            return RedirectToAction("GetCourses");
        }

        public IActionResult Users()
        {
            var users = _unitOfWork.Users.GetAll().ToList();

            return View(users);
        }
      
        public IActionResult Categories()
        {
            var categories = _unitOfWork.Categories.GetAll().ToList();
            return View(categories);
        }

        [HttpPost]
        public IActionResult DeleteCategoryById(int id)
        {
            var category = _unitOfWork.Categories.Get(id);
            _unitOfWork.Categories.Delete(category);
            _unitOfWork.SaveChanges();
            return RedirectToAction("Categories");
        }

        [HttpGet]
        public IActionResult AddTeacher()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddTeacher(RegisterViewModel model, IFormFile profilePhoto)
        {
            if (ModelState.IsValid)
            {
                var imageURL = ProcessUploadFile(profilePhoto);
                var user = new ApplicationUser
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    Email = model.Email,
                    UserName = model.UserName,
                    ProfileImageUrl = imageURL
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync("Teacher"))
                    {
                        var role = new IdentityRole("Teacher");
                        await _roleManager.CreateAsync(role);
                    }
                    await _userManager.AddToRoleAsync(user, "Teacher");
                    return RedirectToAction(nameof(Users));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult DeleteUserById(string id)
        {
            var user = _unitOfWork.Users.Find(t => t.Id == id).First();
            if(user != null)
            {
                _unitOfWork.Users.Delete(user);
                _unitOfWork.SaveChanges();
            }
            return RedirectToAction("Users");
        }

        public IActionResult AddCourse()
        {
            var categories = _unitOfWork.Categories.GetAll().ToList();
            return View(categories);
        }

        [HttpPost]
        public IActionResult AddNewCourse(int categoryId, string courseName)
        {
            var course = new Course
            {
                CategoryId = categoryId,
                Name = courseName
            };
            _unitOfWork.Courses.Add(course);
            _unitOfWork.SaveChanges();
            return RedirectToAction("GetCourses", "Admin");
        }

        [HttpPost]
        public IActionResult AddNewCategory(string categoryName,string type)
        {
            var category = new Category
            {
                Name = categoryName,
                Type = type
            };
            _unitOfWork.Categories.Add(category);
            _unitOfWork.SaveChanges();
            return RedirectToAction("GetCourses", "Admin");
        }





        private string ProcessUploadFile(IFormFile photo)
        {
            string result = null;
            if (photo != null)
            {
                var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath + "\\app-assets\\images\\backgrounds");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                photo.CopyTo(new FileStream(filePath, FileMode.Create));
                result = "/app-assets/images/backgrounds/" + uniqueFileName;
            }
            else
            {
                result = "/app-assets/images/backgrounds/" + "default-profile-picture.jpg";
            }
            return result;
        }
    }
}