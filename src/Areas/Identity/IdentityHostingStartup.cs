using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SuxrobGM_Resume.Data;
using SuxrobGM_Resume.Models;

[assembly: HostingStartup(typeof(SuxrobGM_Resume.Areas.Identity.IdentityHostingStartup))]
namespace SuxrobGM_Resume.Areas.Identity
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