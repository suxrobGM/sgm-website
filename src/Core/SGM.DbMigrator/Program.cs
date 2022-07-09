using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SGM.DbMigrator;
using SGM.EntityFramework;
using SGM.EntityFramework.Data;

await RunAsync();

async Task RunAsync()
{
    var connectionString = ConnectionStrings.Local;
    var configuration = GetConfiguration();
    var services = new ServiceCollection();
    services.AddInfrastructureLayer(configuration);
    var serviceProvider = services.BuildServiceProvider();


    Console.WriteLine("Connection string: " + connectionString);
    Console.WriteLine("Initializing database...");

    await SeedData.InitializeAsync<DatabaseContext>(serviceProvider);

    Console.WriteLine("Finished!");
    Console.ReadLine();
}

IConfigurationRoot GetConfiguration()
{
    var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

    return configuration;
}
