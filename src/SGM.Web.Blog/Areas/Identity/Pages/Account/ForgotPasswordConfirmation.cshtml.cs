using Microsoft.AspNetCore.Authorization;

namespace SGM.BlogApp.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class ForgotPasswordConfirmation : PageModel
{
    public void OnGet()
    {
    }
}
