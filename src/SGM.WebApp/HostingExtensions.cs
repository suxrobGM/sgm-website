using SGM.Application;
using SuxrobGM.Sdk.ServerAnalytics;
using SuxrobGM.Sdk.ServerAnalytics.Sqlite;

namespace SGM.WebApp;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        AddSecretsJson(builder.Configuration);
        builder.Services.AddApplicationLayer(builder.Configuration);
        builder.Services.AddScoped(_ => new SqliteDbContext(builder.Configuration.GetConnectionString("AnalyticsSqliteDB")));
        builder.Services.AddRazorPages();
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
        var path = Path.Combine(AppContext.BaseDirectory, "secrets.json");
        configuration.AddJsonFile(path, true);
    }
}
