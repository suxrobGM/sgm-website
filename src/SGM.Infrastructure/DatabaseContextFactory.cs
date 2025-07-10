using SGM.Infrastructure;
using Microsoft.EntityFrameworkCore.Design;
using SGM.Infrastructure.Data;

namespace SGM.EntityFramework.Data;

public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        const string connectionString = ConnectionStrings.Local;
        return new DatabaseContext(connectionString);
    }
}
