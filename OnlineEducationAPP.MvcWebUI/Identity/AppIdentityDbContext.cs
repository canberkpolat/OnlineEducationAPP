using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineEducationAPP.MvcWebUI.Identity
{
    public class OnlineEducationDbContext : IdentityDbContext<IdentityUser,IdentityRole ,string>
    {
        public OnlineEducationDbContext(DbContextOptions<OnlineEducationDbContext> options) : base(options)
        {

        }
        
    }
}
