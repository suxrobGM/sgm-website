using Microsoft.AspNetCore.Authorization;

namespace SGM.BlogApp.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class ResetPasswordConfirmationModel : PageModel
{
    public void OnGet()
    {

    }
}
