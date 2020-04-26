using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineEducationAPP.MvcWebUI.Identity;
using OnlineEducationAPP.MvcWebUI.Models;

namespace OnlineEducationAPP.MvcWebUI.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ProfileController(UserManager<ApplicationUser> userManager , IHostingEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Detail(string id)
        {

            var user = await _userManager.FindByIdAsync(id);
            var model = new UserEditViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                UserName = user.UserName,
                ProfileImageUrl = user.ProfileImageUrl,
                CarouselImageUrl = user.CarouselImageUrl
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {

                var user = await _userManager.FindByIdAsync(model.Id);

                user.Name = model.Name;
                user.Surname = model.Surname;
                user.Email = model.Email;
                user.UserName = model.UserName;
                if(model.ProfilePhoto !=null)
                    user.ProfileImageUrl = ProcessUploadFile(model)[0];
                if(model.CarouselPhoto != null)
                    user.CarouselImageUrl = ProcessUploadFile(model)[1];

                await _userManager.UpdateAsync(user);

            }
            return RedirectToAction("Index","Dashboard");
        }

        private string[] ProcessUploadFile(UserEditViewModel model)
        {
            string[] result = new string[2];
            string uniqueFileName = null;
            if (model.ProfilePhoto != null)
            {
                var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath + "\\app-assets\\images\\backgrounds");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfilePhoto.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                model.ProfilePhoto.CopyTo(new FileStream(filePath, FileMode.Create));
                result[0] = "/app-assets/images/backgrounds/" + uniqueFileName;
            }
            uniqueFileName = null;
            if (model.CarouselPhoto != null)
            {
                var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath + "\\app-assets\\images\\backgrounds");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.CarouselPhoto.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                model.CarouselPhoto.CopyTo(new FileStream(filePath, FileMode.Create));
                result[1] = "/app-assets/images/backgrounds/" + uniqueFileName;
            }
            return result;
        }
    }
}