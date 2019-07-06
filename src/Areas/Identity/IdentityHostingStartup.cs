using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(SuxrobGM_Website.Areas.Identity.IdentityHostingStartup))]
namespace SuxrobGM_Website.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}