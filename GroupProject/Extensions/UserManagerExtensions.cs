using GroupProject.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroupProject.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<bool> ToggleUserActive(this UserManager<ApplicationUser> userManager, string userId)
        {
            var x = await userManager.FindByIdAsync(userId);
            x.Deactivated = !x.Deactivated;
            var result = await userManager.UpdateAsync(x);
            if (result.Succeeded) return true;
            return false;
        }
        public static async Task<bool> IsAccountDeactivated(this UserManager<ApplicationUser> userManager, string userId)
        {
            return (await userManager.FindByIdAsync(userId)).Deactivated;
        }


    }
}
