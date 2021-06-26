using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Settings.Configuration;

namespace SGM.Web
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
                Log.Logger?.Information("Started webapp SGM Profile");
                CreateHostBuilder(args).Build().Run();
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
                .ReadFrom.Configuration(configuration, ConfigurationAssemblySource.AlwaysScanDllFiles)
                .CreateLogger();
        }

        #endregion
    }
}
