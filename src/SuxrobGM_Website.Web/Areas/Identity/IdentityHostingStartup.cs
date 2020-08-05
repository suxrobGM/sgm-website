using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(SuxrobGM_Website.Web.Areas.Identity.IdentityHostingStartup))]
namespace SuxrobGM_Website.Web.Areas.Identity
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