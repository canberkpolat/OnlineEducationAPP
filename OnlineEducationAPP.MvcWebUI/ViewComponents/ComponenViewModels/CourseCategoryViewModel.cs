using OnlineEducationAPP.MvcWebUI.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineEducationAPP.MvcWebUI.ViewComponents
{
    public class CourseCategoryViewModel
    {
        public List<Course> Courses { get; set; }
        public List<Category> Categories { get; set; }
    }
}
