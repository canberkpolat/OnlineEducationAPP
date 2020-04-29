using OnlineEducationAPP.MvcWebUI.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineEducationAPP.MvcWebUI.Entity
{
    public class UserProfile
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string ProfileImageUrl { get; set; }
        public string CarouselImageUrl { get; set; }


        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}
