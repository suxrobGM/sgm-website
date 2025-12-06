using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using SGM.WebApp.Options;

namespace SGM.WebApp.Components.Pages;

public partial class Home
{
    [Inject]
    private IOptions<GoogleRecaptchaOptions> RecaptchaOptions { get; set; } = default!;

    [Inject]
    private IJSRuntime JS { get; set; } = default!;

    private const string Description = "Senior Full Stack Developer and AI Researcher with 9+ years of experience. M.S. Computer Science at Northeastern University. Specializing in Computer Vision, Deep Learning, and Full-Stack Development.";

    private string CaptchaSiteKey => RecaptchaOptions.Value.SiteKey;
}
