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
            try
            {
                await CreateUserRolesAsync(service);
                await CreateOwnerAccountAsync(service);
                await CreateDeletedUserAccountAsync(service);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private static async Task CreateUserRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            var superAdminRole = await roleManager.RoleExistsAsync(Role.SuperAdmin.ToString());
            var adminRole = await roleManager.RoleExistsAsync(Role.Admin.ToString());
            var moderatorRole = await roleManager.RoleExistsAsync(Role.Moderator.ToString());
            var editorRole = await roleManager.RoleExistsAsync(Role.Editor.ToString());

            if (!superAdminRole)
            {
                await roleManager.CreateAsync(new ApplicationRole(Role.SuperAdmin));
            }
            if (!adminRole)
            {
                await roleManager.CreateAsync(new ApplicationRole(Role.Admin));
            }
            if (!moderatorRole)
            {
                await roleManager.CreateAsync(new ApplicationRole(Role.Moderator));
            }
            if (!editorRole)
            {
                await roleManager.CreateAsync(new ApplicationRole(Role.Editor));
            }
        }

        private static async Task CreateOwnerAccountAsync(IServiceProvider service)
        {
            var userManager = service.GetRequiredService<UserManager<ApplicationUser>>();
            var config = service.GetRequiredService<IConfiguration>();

            var ownerAccount = new ApplicationUser()
            {
                UserName = config.GetSection("ServerAccounts:Owner:UserName").Value,
                Email = config.GetSection("ServerAccounts:Owner:Email").Value,
                EmailConfirmed = true
            };
            var password = config.GetSection("ServerAccounts:Owner:Password").Value;

            var siteOwner = await userManager.FindByEmailAsync(ownerAccount.Email);
            if (siteOwner == null)
            {
                await userManager.CreateAsync(ownerAccount, password);
            }

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

            var deletedUserAccount = new ApplicationUser()
            {
                UserName = config.GetSection("ServerAccounts:DeletedUser:UserName").Value,
                Email = config.GetSection("ServerAccounts:DeletedUser:Email").Value,
                EmailConfirmed = true
            };
            var password = config.GetSection("ServerAccounts:DeletedUser:Password").Value;

            var deletedUser = await userManager.FindByNameAsync(deletedUserAccount.UserName);
            if (deletedUser == null)
            {
                await userManager.CreateAsync(deletedUserAccount, password);
            }

            var hasSuperAdminRole = await userManager.IsInRoleAsync(deletedUser, Role.SuperAdmin.ToString());

            if (!hasSuperAdminRole)
            {
                await userManager.AddToRoleAsync(deletedUser, Role.SuperAdmin.ToString());
            }
        }
    }
}
