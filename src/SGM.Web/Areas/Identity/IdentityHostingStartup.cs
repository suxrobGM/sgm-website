using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(SGM.Web.Areas.Identity.IdentityHostingStartup))]
namespace SGM.Web.Areas.Identity
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