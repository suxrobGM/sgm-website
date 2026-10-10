using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using SGM.WebApp.Options;
using SGM.WebApp.Services;

namespace SGM.WebApp.Components.Pages;

public abstract class HomePageBase : ComponentBase
{
    protected const string Description =
        "Sukhrob Ilyosbekov is a software engineer and computer vision researcher in Portland, " +
        "Maine. Projects, papers, and a way to get in touch.";

    [Inject]
    protected IOptions<GoogleRecaptchaOptions> RecaptchaOptions { get; set; } = null!;

    [Inject]
    protected StaticAssetVersion AssetVersion { get; set; } = null!;

    protected string CaptchaSiteKey => RecaptchaOptions.Value.SiteKey;

    protected static int CurrentYear => DateTime.Now.Year;
}
