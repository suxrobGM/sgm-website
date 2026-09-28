using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using SGM.WebApp.Options;
using SGM.WebApp.Services;

namespace SGM.WebApp.Components.Pages;

public abstract class HomePageBase : ComponentBase
{
    protected const string Description =
        "Sukhrob Ilyosbekov builds software and does computer vision research. Nine years of " +
        "full-stack and ML work, an M.S. in Computer Science from Northeastern, and papers on " +
        "medical imaging and cell microscopy.";

    [Inject]
    protected IOptions<GoogleRecaptchaOptions> RecaptchaOptions { get; set; } = null!;

    [Inject]
    protected IJSRuntime JS { get; set; } = null!;

    [Inject]
    protected StaticAssetVersion Assets { get; set; } = null!;

    protected string CaptchaSiteKey => RecaptchaOptions.Value.SiteKey;

    protected static int CurrentYear => DateTime.Now.Year;
}
