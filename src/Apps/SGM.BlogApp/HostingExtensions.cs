using SGM.Application;
using SGM.BlogApp.Utils;
using SGM.Infrastructure;
using SuxrobGM.Sdk.ServerAnalytics;
using SuxrobGM.Sdk.ServerAnalytics.Sqlite;
using Syncfusion.Licensing;

namespace SGM.BlogApp;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        AddSecretsJson(builder.Configuration);
        SyncfusionLicenseProvider.RegisterLicense(builder.Configuration.GetSection("SynLicenseKey").Value);

        builder.Services.AddApplicationLayer(builder.Configuration);
        builder.Services.AddInfrastructureLayer(builder.Configuration, "RemoteDB");

        builder.Services.AddScoped<ImageHelper>();
        builder.Services.AddScoped(_ => new SqliteDbContext(builder.Configuration.GetConnectionString("AnalyticsSqliteDB")));

        builder.Services.AddAuthentication()
            .AddGoogle(options =>
            {
                var googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");
                options.ClientId = googleAuthNSection["ClientId"];
                options.ClientSecret = googleAuthNSection["ClientSecret"];
            })
            .AddFacebook(options =>
            {
                var facebookAuthSection = builder.Configuration.GetSection("Authentication:Facebook");
                options.AppId = facebookAuthSection["AppId"];
                options.AppSecret = facebookAuthSection["AppSecret"];
            });

        builder.Services.Configure<CookiePolicyOptions>(options =>
        {
            options.CheckConsentNeeded = _ => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromDays(5);
            options.SlidingExpiration = true;
        });

        builder.Services.AddRouting(options => options.LowercaseUrls = true);
        builder.Services.AddRazorPages(options =>
        {
            options.Conventions.AddPageRoute("/Blog/List", "/");
            options.Conventions.AddPageRoute("/Blog/Index", "/{slug}");
        });
        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
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
        app.MapRazorPages();
        return app;
    }

    private static void AddSecretsJson(IConfigurationBuilder configuration)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "appsettings.secrets.json");
        configuration.AddJsonFile(path, true);
    }
}
