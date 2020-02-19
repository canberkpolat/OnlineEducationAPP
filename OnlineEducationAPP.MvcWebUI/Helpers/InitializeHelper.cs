using Microsoft.AspNetCore.Identity;
using OnlineEducationAPP.MvcWebUI.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineEducationAPP.MvcWebUI.Helpers
{
    public static class InitializeHelper
    {
        public static async Task Initial(RoleManager<IdentityRole> roleManager)
        {
            if(!await roleManager.RoleExistsAsync("Admin"))
            {
                var role = new IdentityRole("Admin");
                await roleManager.CreateAsync(role);
            }
            if(!await roleManager.RoleExistsAsync("Student"))
            {
                var role = new IdentityRole("Student");
                await roleManager.CreateAsync(role);
            }
            if (!await roleManager.RoleExistsAsync("Teacher"))
            {
                var role = new IdentityRole("Teacher");
                await roleManager.CreateAsync(role);
            }
        }
             
    }
}
