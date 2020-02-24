using Microsoft.AspNetCore.Mvc;
using OnlineEducationAPP.MvcWebUI.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineEducationAPP.MvcWebUI.ViewComponents
{
    public class CourseList : ViewComponent
    {
        private IUnitOfWork unitOfWork;

        public CourseList(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public IViewComponentResult Invoke()
        {
            var courses = unitOfWork.Courses.GetAll();
            var categories = unitOfWork.Categories.GetAll();

            var model = new CourseCategoryViewModel();
            
            model.Categories = categories.ToList();
            model.Courses = courses.ToList();
            
            return View(model);
        }

    }
}
