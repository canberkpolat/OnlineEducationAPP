using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineEducationAPP.MvcWebUI.Models
{
    public class ProfileViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ProfileImageUrl { get; set; }
        public string CarouselImageUrl { get; set; }
    }
}
