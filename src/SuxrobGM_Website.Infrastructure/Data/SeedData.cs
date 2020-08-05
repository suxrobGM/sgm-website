using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SuxrobGM_Website.Core.Entities.UserEntities;

namespace SuxrobGM_Website.Infrastructure.Data
{
    public class SeedData
    {
        public static async void Initialize(IServiceProvider service)
        {
            await CreateUserRolesAsync(service);
            await AddSuperAdminRoleToSiteOwnerAsync(service);
            await CreateDeletedUserAccountAsync(service);
        }

        private static async Task CreateUserRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<UserRole>>();

            var superAdminRole = await roleManager.RoleExistsAsync(Role.SuperAdmin.ToString());
            var adminRole = await roleManager.RoleExistsAsync(Role.Admin.ToString());
            var moderatorRole = await roleManager.RoleExistsAsync(Role.Moderator.ToString());
            var editorRole = await roleManager.RoleExistsAsync(Role.Editor.ToString());

            if (!superAdminRole)
            {
                await roleManager.CreateAsync(new UserRole(Role.SuperAdmin));
            }
            if (!adminRole)
            {
                await roleManager.CreateAsync(new UserRole(Role.Admin));
            }
            if (!moderatorRole)
            {
                await roleManager.CreateAsync(new UserRole(Role.Moderator));
            }
            if (!editorRole)
            {
                await roleManager.CreateAsync(new UserRole(Role.Editor));
            }
        }

        private static async Task AddSuperAdminRoleToSiteOwnerAsync(IServiceProvider service)
        {
            var userManager = service.GetRequiredService<UserManager<ApplicationUser>>();
            var siteOwner = await userManager.FindByEmailAsync("suxrobgm@gmail.com");

            if (siteOwner == null) 
                return;

            var hasSuperAdminRole = await userManager.IsInRoleAsync(siteOwner, Role.SuperAdmin.ToString());

            if (!hasSuperAdminRole)
            {
                await userManager.AddToRoleAsync(siteOwner, Role.SuperAdmin.ToString());
            }
        }

        private static async Task CreateDeletedUserAccountAsync(IServiceProvider service)
        {
            var userManager = service.GetRequiredService<UserManager<ApplicationUser>>();
            var config = service.GetRequiredService<IConfiguration>();

            var deletedUserAccount = await userManager.FindByNameAsync("DELETED_USER");
            if (deletedUserAccount == null)
            {
                await userManager.CreateAsync(new ApplicationUser()
                {
                    UserName = "DELETED_USER",
                    Email = "Deleted.User@suxrobgm.net",
                    EmailConfirmed = true
                }, 
                config.GetSection("EmailPassword").Value);
            }

            var hasSuperAdminRole = await userManager.IsInRoleAsync(deletedUserAccount, Role.SuperAdmin.ToString());

            if (!hasSuperAdminRole)
            {
                await userManager.AddToRoleAsync(deletedUserAccount, Role.SuperAdmin.ToString());
            }
        }
    }
}
