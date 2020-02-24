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

        public Stream Stream { get; set; }
    }
}
