﻿using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(SGM.BlogApp.Areas.Identity.IdentityHostingStartup))]
namespace SGM.BlogApp.Areas.Identity;

public class IdentityHostingStartup : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureServices((context, services) => {
        });
    }
}
