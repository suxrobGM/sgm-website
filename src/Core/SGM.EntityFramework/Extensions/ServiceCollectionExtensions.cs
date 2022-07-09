using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SGM.EntityFramework;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureLayer(
        this IServiceCollection services,
        IConfiguration configuration,
        string connectionStringName = "LocalDB")
    {
        var connectionString = configuration.GetConnectionString(connectionStringName);

        services.AddDbContext<DatabaseContext>(
            o => DbContextHelpers.ConfigureSqlServer(connectionString, o));

        ConfigureIdentity(services);
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IBlogRepository, BlogRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }

    private static void ConfigureIdentity(IServiceCollection services)
    {
        services.AddDefaultIdentity<ApplicationUser>()
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<DatabaseContext>();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.User.AllowedUserNameCharacters = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM0123456789_.-";
            options.User.RequireUniqueEmail = true;
            //options.SignIn.RequireConfirmedAccount = true;
        });
    }
}