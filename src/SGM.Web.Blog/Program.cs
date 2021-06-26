using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SGM.Infrastructure.Data;

namespace SGM.Web.Blog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = BuildConfiguration();
            Log.Logger = CreateLogger(configuration);

            try
            {
                Log.Logger?.Information("-------------------------------------------------");
                Log.Logger?.Information("Started webapp SGM Blog");
                
                var host = CreateHostBuilder(args).Build();
                using var scope = host.Services.CreateScope();
                var serviceProvider = scope.ServiceProvider;
                SeedData.Initialize(serviceProvider);

                host.Run();
            }
            catch (Exception ex)
            {
                Log.Logger?.Fatal(ex, "Application start-up failed");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        
        #region Static methods for creating logger 

        private static IConfiguration BuildConfiguration()
        {
            Environment.SetEnvironmentVariable("BASEDIR", AppContext.BaseDirectory);
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            return new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .Build();
        }

        private static ILogger CreateLogger(IConfiguration configuration)
        {
            return new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        #endregion
    }
}
