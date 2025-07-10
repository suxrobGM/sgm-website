using Microsoft.EntityFrameworkCore;

namespace SGM.Infrastructure.Helpers;

internal static class DbContextHelpers
{
    public static void ConfigureSqlServer(string connectionString, DbContextOptionsBuilder options)
    {
        options.UseSqlServer(connectionString).UseLazyLoadingProxies();
    }
}
