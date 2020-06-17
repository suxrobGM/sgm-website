using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

        public void ConfigureServices(IServiceCollection services)
        {
            SyncfusionLicenseProvider.RegisterLicense(Configuration.GetSection("SynLicenseKey").Value);

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
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
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("RemoteDbConnection"))
                    .UseLazyLoadingProxies();
            });
            services.AddScoped<SqliteDbContext>(_ => new SqliteDbContext(Configuration.GetConnectionString("AnalyticsSqliteDbConnection")));

            services.AddDefaultIdentity<User>()
                .AddRoles<UserRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    var googleAuthSection = Configuration.GetSection("Authentication:Google");
                    options.ClientId = googleAuthSection["ClientId"];
                    options.ClientSecret = googleAuthSection["ClientSecret"];
                })
                .AddFacebook(options =>
                {
                    var facebookAuthSection = Configuration.GetSection("Authentication:Facebook");
                    options.AppId = facebookAuthSection["AppId"];
                    options.AppSecret = facebookAuthSection["AppSecret"];
                });

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

            services.AddTransient<IEmailSender, EmailSender>(_ => new EmailSender(Configuration));
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddControllers();
            services.AddRazorPages(options =>
            {
                options.Conventions.AddPageRoute("/Blog/List", "/Blog");
            });            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {               
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");                
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseServerAnalytics(new SqliteAnalyticsRepository())
                .ExcludePath("/js", "/lib", "/css", "/fonts", "/wp-includes", "/wp-admin", "/wp-includes/")
                .ExcludeExtension(".jpg", ".png", ".ico", ".txt", ".php", "sitemap.xml", "sitemap.xsl")
                .ExcludeLoopBack()
                .Exclude(ctx => ctx.Request.Headers["User-Agent"].ToString().ToLower().Contains("bot"));

            app.UseStaticFiles(new StaticFileOptions()
            {
                HttpsCompression = Microsoft.AspNetCore.Http.Features.HttpsCompressionMode.Compress,
                OnPrepareResponse = context =>
                {
                    var headers = context.Context.Response.GetTypedHeaders();
                    headers.CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromDays(30)
                    };
                }
            });
            app.UseRouting();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }       
    }
}
