using Microsoft.AspNetCore.Authorization;

namespace SGM.BlogApp.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class LockoutModel : PageModel
{
    public void OnGet()
    {

    }
}
