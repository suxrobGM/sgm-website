using Microsoft.EntityFrameworkCore;
using SGM.DbMigrator;
using SGM.Infrastructure.Data;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(configuration =>
    {
        var path = Path.Combine(AppContext.BaseDirectory, "secrets.json");
        configuration.AddJsonFile(path, true);
    })
    .ConfigureServices((ctx, services) =>
    {
        var connectiongString = ctx.Configuration.GetConnectionString("LocalDB");
        services.AddDbContext<DatabaseContext>(o => o.UseSqlServer(connectiongString));
        services.AddHostedService<SeedDataService>();
    })
    .Build();

await host.RunAsync();
