using Microsoft.EntityFrameworkCore;
using SuxrobGM.Sdk.ServerAnalytics;
using SuxrobGM.Sdk.ServerAnalytics.Sqlite;
using Syncfusion.Licensing;
using SGM.BlogApp.Utils;
using SGM.Application;
using SGM.EntityFramework;

namespace SGM.BlogApp;

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

        services.AddApplicationLayer(Configuration);
        services.AddInfrastructureLayer(Configuration, "RemoteDB");

        services.AddScoped<ImageHelper>();
        services.AddScoped(_ => new SqliteDbContext(Configuration.GetConnectionString("AnalyticsSqliteDB")));

        services.AddAuthentication()
            .AddGoogle(options =>
            {
                var googleAuthNSection = Configuration.GetSection("Authentication:Google");
                options.ClientId = googleAuthNSection["ClientId"];
                options.ClientSecret = googleAuthNSection["ClientSecret"];
            })
            .AddFacebook(options =>
            {
                var facebookAuthSection = Configuration.GetSection("Authentication:Facebook");
                options.AppId = facebookAuthSection["AppId"];
                options.AppSecret = facebookAuthSection["AppSecret"];
            });

        services.Configure<CookiePolicyOptions>(options =>
        {
            options.CheckConsentNeeded = _ => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });
        services.ConfigureApplicationCookie(options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromDays(5);
            options.SlidingExpiration = true;
        });

        services.AddRouting(options => options.LowercaseUrls = true);
        services.AddRazorPages(options =>
        {
            options.Conventions.AddPageRoute("/Blog/List", "/");
            options.Conventions.AddPageRoute("/Blog/Index", "/{slug}");
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
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

        app.UseStaticFiles();
        app.UseRouting();
        app.UseCookiePolicy();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
        });
    }
}
