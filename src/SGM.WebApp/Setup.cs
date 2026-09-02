using SGM.WebApp.Components;
using SGM.WebApp.Options;
using SGM.WebApp.Services;

namespace SGM.WebApp;

internal static class Setup
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddOptions<EmailSenderOptions>().BindConfiguration("EmailConfig");
        builder.Services.AddOptions<GoogleRecaptchaOptions>().BindConfiguration("GoogleRecaptcha");
        builder.Services.AddOptions<FreeKassaOptions>().BindConfiguration("FreeKassa");

        builder.Services.AddScoped<IEmailSender, EmailSender>();
        builder.Services.AddScoped<ICaptchaService, RecaptchaEnterpriseService>();
        builder.Services.AddHttpClient<IFreeKassaService, FreeKassaService>();
        builder.Services.AddSingleton<StaticAssetVersion>();

        builder.Services.AddControllers();
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

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

        app.UseStaticFiles(new StaticFileOptions
        {
            OnPrepareResponse = ctx =>
            {
                // Resume PDFs are replaced in place, so force revalidation; unchanged files still 304 via ETag.
                if (ctx.File.Name.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                {
                    ctx.Context.Response.Headers.CacheControl = "no-cache, must-revalidate";
                }
            },
        });
        app.UseRouting();
        app.UseCookiePolicy();
        app.UseAntiforgery();

        app.MapControllers();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        return app;
    }
}
