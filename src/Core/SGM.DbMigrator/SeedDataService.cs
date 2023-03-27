using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SGM.Domain.Entities.UserEntities;
using SGM.EntityFramework.Data;

namespace SGM.DbMigrator
{
    public class SeedDataService : BackgroundService
    {
        private readonly ILogger<SeedDataService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public SeedDataService(
            ILogger<SeedDataService> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            _logger.LogInformation("Initializing database");
            await MigrateDatabaseAsync(databaseContext);
            _logger.LogInformation("Successfully initialized the database");

            _logger.LogInformation("Creating default user roles");
            await CreateUserRolesAsync(scope.ServiceProvider);
            _logger.LogInformation("Successfully created default user roles");

            _logger.LogInformation("Creating admin account");
            await CreateDefaultAdminAsync(scope.ServiceProvider);
            _logger.LogInformation("Successfully created admin account");

            _logger.LogInformation("Creating deleted user account");
            await CreateDeletedUserAccountAsync(scope.ServiceProvider);
            _logger.LogInformation("Successfully created deleted user account");
            _logger.LogInformation("Successfully seeded databases");
        }

        private async Task MigrateDatabaseAsync(DbContext databaseContext)
        {
            try
            {
                await databaseContext.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Thrown exception in SeedData.MigrateDatabaseAsync(): {Exception}", ex);
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

        private static async Task CreateDefaultAdminAsync(IServiceProvider service)
        {
            var userManager = service.GetRequiredService<UserManager<ApplicationUser>>();
            var config = service.GetRequiredService<IConfiguration>();

            var ownerAccount = new ApplicationUser()
            {
                UserName = config["ServerAccounts:Admin:UserName"],
                Email = config["ServerAccounts:Admin:Email"],
                EmailConfirmed = true
            };
            var password = config["ServerAccounts:Admin:Password"];

            var siteOwner = await userManager.FindByEmailAsync(ownerAccount.Email!);
            if (siteOwner == null)
            {
                await userManager.CreateAsync(ownerAccount, password!);
                siteOwner = await userManager.FindByEmailAsync(ownerAccount.Email!);
            }

            var hasSuperAdminRole = await userManager.IsInRoleAsync(siteOwner!, Role.SuperAdmin.ToString());

            if (!hasSuperAdminRole)
            {
                await userManager.AddToRoleAsync(siteOwner!, Role.SuperAdmin.ToString());
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

            var deletedUser = await userManager.FindByNameAsync(deletedUserAccount.UserName!);
            if (deletedUser == null)
            {
                await userManager.CreateAsync(deletedUserAccount, password!);
                deletedUser = await userManager.FindByNameAsync(deletedUserAccount.UserName!);
            }

            var hasSuperAdminRole = await userManager.IsInRoleAsync(deletedUser!, Role.SuperAdmin.ToString());

            if (!hasSuperAdminRole)
            {
                await userManager.AddToRoleAsync(deletedUser!, Role.SuperAdmin.ToString());
            }
        }
    }
}