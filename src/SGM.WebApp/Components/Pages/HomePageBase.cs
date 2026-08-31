using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using SGM.WebApp.Options;

namespace SGM.WebApp.Components.Pages;

public abstract class HomePageBase : ComponentBase
{
    protected const string Description =
        "Machine learning engineer and computer vision researcher with three published papers " +
        "and 9+ years building production software. M.S. in Computer Science from Northeastern " +
        "University. Deep learning, explainable AI, medical imaging, and LLM systems.";

    [Inject]
    protected IOptions<GoogleRecaptchaOptions> RecaptchaOptions { get; set; } = null!;

    [Inject]
    protected IJSRuntime JS { get; set; } = null!;

    protected string CaptchaSiteKey => RecaptchaOptions.Value.SiteKey;

    protected static int CurrentYear => DateTime.Now.Year;
}
