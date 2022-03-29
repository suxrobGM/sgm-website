using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace SGM.BlogApp.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class LogoutModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<LogoutModel> _logger;

    public LogoutModel(SignInManager<ApplicationUser> signInManager, ILogger<LogoutModel> logger)
    {
        _signInManager = signInManager;
        _logger = logger;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = "/")
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logged out");
        return LocalRedirect(returnUrl);
    }
}
