using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityService.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Infrastructure.Seed
{
    public class UserSeeder
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            var admin = await userManager.FindByEmailAsync("admin@admin.com");
            if (admin == null)
            {
                var createAdmin = new ApplicationUser
                {
                    Email = "admin@admin.com",
                    UserName = "admin@admin.com",
                    FirstName = "Admin",
                    LastName = "Admin",
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(createAdmin, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(createAdmin, "Admin");
                }
                else
                {
                    foreach (var error in result.Errors)
                        Console.WriteLine(error.Description);
                }
            }
        }
    }
}
