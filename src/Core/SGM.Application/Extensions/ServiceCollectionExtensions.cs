using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SGM.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationLayer(
        this IServiceCollection services,
        IConfiguration configuration,
        string recaptchaSection = "GoogleRecaptcha",
        string emailConfigSection = "EmailConfig")
    {
        var captchaOptions = configuration.GetSection(recaptchaSection).Get<GoogleRecaptchaOptions>();
        var emailSenderOptions = configuration.GetSection(emailConfigSection).Get<EmailSenderOptions>();
        
        if (captchaOptions != null)
        {
            services.AddSingleton(captchaOptions);
        }

        if (emailSenderOptions != null)
        {
            services.AddSingleton(emailSenderOptions);
        }
        
        services.AddTransient<IEmailSender, EmailSender>();
        services.AddScoped<ICaptchaService, GoogleRecaptchaService>();
        return services;
    }
}