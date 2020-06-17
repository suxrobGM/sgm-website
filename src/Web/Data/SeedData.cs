using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SuxrobGM_Website.Models;

namespace SuxrobGM_Website.Data
{
    public class SeedData
    {
        public static async void CreateUserRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<UserRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            var superAdminRole = await roleManager.RoleExistsAsync(Role.SuperAdmin.ToString());
            var adminRole = await roleManager.RoleExistsAsync(Role.Admin.ToString());
            var moderatorRole = await roleManager.RoleExistsAsync(Role.Moderator.ToString());
            var editorRole = await roleManager.RoleExistsAsync(Role.Editor.ToString());

            if (!superAdminRole)
            {
                var roleResult = await roleManager.CreateAsync(new UserRole(Role.SuperAdmin));
            }
            if (!adminRole)
            {
                var roleResult = await roleManager.CreateAsync(new UserRole(Role.Admin));
            }
            if (!moderatorRole)
            {
                var roleResult = await roleManager.CreateAsync(new UserRole(Role.Moderator));
            }
            if (!editorRole)
            {
                var roleResult = await roleManager.CreateAsync(new UserRole(Role.Editor));
            }

            var admin = await userManager.FindByEmailAsync("suxrobgm@gmail.com");

            if (admin != null)
            {
                var hasSuperAdminRole = await userManager.IsInRoleAsync(admin, Role.SuperAdmin.ToString());

                if (!hasSuperAdminRole)
                {
                    await userManager.AddToRoleAsync(admin, Role.SuperAdmin.ToString());
                }
            }          
        }
    }
}
