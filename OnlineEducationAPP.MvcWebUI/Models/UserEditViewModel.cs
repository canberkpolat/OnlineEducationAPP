using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineEducationAPP.MvcWebUI.Models
{
    public class UserEditViewModel
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ProfileImageUrl { get; set; }
        public string CarouselImageUrl { get; set; }
        public IFormFile ProfilePhoto { get; set; }
        public IFormFile CarouselPhoto { get; set; }

    }
}
