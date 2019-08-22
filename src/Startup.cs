using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Syncfusion.Licensing;
using SuxrobGM.Sdk.ServerAnalytics;
using SuxrobGM.Sdk.ServerAnalytics.Sqlite;
using SuxrobGM_Website.Data;
using SuxrobGM_Website.Models;
using SuxrobGM_Website.Services;

namespace SuxrobGM_Website
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            SyncfusionLicenseProvider.RegisterLicense(Configuration.GetConnectionString("SynLicenseKey"));

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.All;
                options.KnownProxies.Add(IPAddress.Parse("::ffff:172.16.1.1"));
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(5);
                options.SlidingExpiration = true;
            });
            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(3);
            });               
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AddPageRoute("/Blog/List", "/Blog");
                });

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("RemoteConnection"))
                    .UseLazyLoadingProxies();
            });
            services.AddDefaultIdentity<User>()
                .AddRoles<UserRole>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                
                //User settings
                options.User.AllowedUserNameCharacters = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM0123456789_.-";
                options.User.RequireUniqueEmail = true;
                //options.SignIn.RequireConfirmedEmail = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseForwardedHeaders();

            if (env.IsDevelopment())
            {               
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");                
                app.UseHsts();
            }

            app.UseServerAnalytics(new SqliteAnalyticsRepository())
                .ExcludePath("/js", "/lib", "/css", "/fonts") // Request into those url spaces will be not recorded
                .ExcludeExtension(".jpg", ".png", ".ico", ".txt", "sitemap.xml", "sitemap.xsl")  // Request ending with this extension will be not recorded
                //.ExcludeLoopBack()  // Request coming from local host will be not recorded
                .Exclude(ctx => ctx.Request.Headers["User-Agent"].ToString().ToLower().Contains("bot")); // Request coming from search engine bots will not be recorded
                            
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();            
            app.UseMvc();
            
            //CreateUserRoles(provider);
            //AddDefaultProfilePhoto(provider);
        }       

        private void CreateUserRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<UserRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            var superAdminRole = roleManager.RoleExistsAsync(Role.SuperAdmin.ToString()).Result;
            var adminRole = roleManager.RoleExistsAsync(Role.Admin.ToString()).Result;
            var moderatorRole = roleManager.RoleExistsAsync(Role.Moderator.ToString()).Result;
            var editorRole = roleManager.RoleExistsAsync(Role.Editor.ToString()).Result;

            if (!superAdminRole)
            {
                var roleResult = roleManager.CreateAsync(new UserRole(Role.SuperAdmin)).Result;
            }
            if (!adminRole)
            {
                var roleResult = roleManager.CreateAsync(new UserRole(Role.Admin)).Result;
            }
            if (!moderatorRole)
            {
                var roleResult = roleManager.CreateAsync(new UserRole(Role.Moderator)).Result;
            }           
            if (!editorRole)
            {
                var roleResult = roleManager.CreateAsync(new UserRole(Role.Editor)).Result;
            }

            User admin = userManager.FindByEmailAsync("suxrobgm@gmail.com").Result;
            userManager.AddToRoleAsync(admin, Role.SuperAdmin.ToString()).Wait();
        }
        private void AddDefaultProfilePhoto(IServiceProvider provider)
        {
            var db = provider.GetRequiredService<ApplicationDbContext>();
            var user = db.Users.Where(i => i.UserName.ToLower() == "suxrobgm").First();
            var defaultUserPhoto = "/img/user_def_icon.png";

            if (string.IsNullOrEmpty(user.ProfilePhotoUrl))
            {
                user.ProfilePhotoUrl = defaultUserPhoto;
            }
            db.SaveChanges();
        }
    }
}
