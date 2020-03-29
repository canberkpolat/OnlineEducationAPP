using Microsoft.AspNetCore.Identity;
using OnlineEducationAPP.MvcWebUI.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineEducationAPP.MvcWebUI.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string ProfileImageUrl { get; set; }
        public string CarouselImageUrl { get; set; }
        public Stream Stream { get; set; }
    }
}
