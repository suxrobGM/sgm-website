using SGM.EntityFramework;
using Microsoft.EntityFrameworkCore.Design;

namespace SGM.EntityFramework.Data;

public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        var connectionString = ConnectionStrings.Local;
        return new DatabaseContext(connectionString);
    }
}