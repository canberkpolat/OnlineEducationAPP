using Microsoft.AspNetCore.Identity;
using OnlineEducationAPP.MvcWebUI.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineEducationAPP.MvcWebUI.Helpers
{
    public static class UserHelper
    {
        public static async Task<string> IsTeacherOrAdmin(UserManager<ApplicationUser> userManager, ApplicationUser user)
        {
            var roles = await userManager.GetRolesAsync(user);

            if (roles.Contains("Teacher") || roles.Contains("Admin"))
            {
                return "true";
            }
            else
            {
                return "false";
            }
        }
    }
}
