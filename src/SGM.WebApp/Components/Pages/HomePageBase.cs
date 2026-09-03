using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using SGM.WebApp.Options;
using SGM.WebApp.Services;

namespace SGM.WebApp.Components.Pages;

public abstract class HomePageBase : ComponentBase
{
    protected const string Description =
        "Machine learning engineer and computer vision researcher working on explainable medical " +
        "imaging, representation learning, and LLM systems, with nine years building production " +
        "software. M.S. in Computer Science from Northeastern University.";

    [Inject]
    protected IOptions<GoogleRecaptchaOptions> RecaptchaOptions { get; set; } = null!;

    [Inject]
    protected IJSRuntime JS { get; set; } = null!;

    [Inject]
    protected StaticAssetVersion Assets { get; set; } = null!;

    protected string CaptchaSiteKey => RecaptchaOptions.Value.SiteKey;

    protected static int CurrentYear => DateTime.Now.Year;
}
