using OnlineEducationAPP.MvcWebUI.Entity;
using OnlineEducationAPP.MvcWebUI.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineEducationAPP.MvcWebUI.Models
{
    public class ActiveStreamViewModel
    {
        public List<Stream> Streams { get; set; }
        public List<ApplicationUser> Users { get; set; }
    }
}
