using SGM.WebApp.Options;
using SGM.WebApp.Services;

namespace SGM.WebApp;

internal static class Setup
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        var emailSenderOptions = builder.Configuration.GetSection("EmailConfig").Get<EmailSenderOptions>();

        if (emailSenderOptions is not null)
        {
            builder.Services.AddSingleton(emailSenderOptions);
        }

        builder.Services.AddOptions<GoogleRecaptchaOptions>().BindConfiguration("GoogleRecaptcha");
        builder.Services.AddScoped<IEmailSender, EmailSender>();
        builder.Services.AddScoped<ICaptchaService, RecaptchaEnterpriseService>();

        builder.Services.AddControllers();
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

        app.UseStaticFiles();
        app.UseRouting();
        app.UseCookiePolicy();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.MapRazorPages();
        return app;
    }
}
