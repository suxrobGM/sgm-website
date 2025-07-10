using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SGM.Application.Options;
using SGM.Application.Services;

namespace SGM.Application;

public static class Registrar
{
    public static IServiceCollection AddApplicationLayer(
        this IServiceCollection services,
        IConfiguration configuration,
        string recaptchaSection = "GoogleRecaptcha",
        string emailConfigSection = "EmailConfig")
    {
        var emailSenderOptions = configuration.GetSection(emailConfigSection).Get<EmailSenderOptions>();

        if (emailSenderOptions != null)
        {
            services.AddSingleton(emailSenderOptions);
        }

        services.AddOptions<GoogleRecaptchaOptions>().BindConfiguration(recaptchaSection);
        services.AddScoped<IEmailSender, EmailSender>();
        services.AddScoped<ICaptchaService, RecaptchaEnterpriseService>();
        return services;
    }
}
