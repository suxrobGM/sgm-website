using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using SGM.WebApp.Options;

namespace SGM.WebApp.Components.Pages;

public abstract class HomePageBase : ComponentBase
{
    protected const string Description =
        "Senior Full Stack Developer and AI Researcher with 9+ years of experience. " +
        "M.S. Computer Science at Northeastern University. " +
        "Specializing in Computer Vision, Deep Learning, and Full-Stack Development.";

    [Inject]
    protected IOptions<GoogleRecaptchaOptions> RecaptchaOptions { get; set; } = null!;

    [Inject]
    protected IJSRuntime JS { get; set; } = null!;

    protected string CaptchaSiteKey => RecaptchaOptions.Value.SiteKey;
}
