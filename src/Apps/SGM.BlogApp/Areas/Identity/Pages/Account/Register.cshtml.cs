using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SGM.Application.Options;
using SGM.Application.Services;

namespace SGM.BlogApp.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class RegisterModel : PageModel
{
    private readonly ICaptchaService _captchaService;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<RegisterModel> _logger;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    

    public RegisterModel(
        ICaptchaService captchaService,
        IEmailSender emailSender,
        ILogger<RegisterModel> logger,
        GoogleRecaptchaOptions recaptchaOptions,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _captchaService = captchaService;
        _emailSender = emailSender;
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        CaptchaSiteKey = recaptchaOptions.SiteKey;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public string ReturnUrl { get; set; }

    public string CaptchaSiteKey { get; }

    public class InputModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public void OnGet(string returnUrl = null)
    {
        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        var captchaValue = HttpContext.Request.Form["g-Recaptcha-Response"].ToString();
        var validCaptcha = await _captchaService.VerifyCaptchaAsync(captchaValue);

        if (!validCaptcha)
            ModelState.AddModelError("captcha", "Invalid captcha");

        if (!ModelState.IsValid) 
            return Page();

        var user = new ApplicationUser
        {
            UserName = Input.UserName,
            Email = Input.Email,
            ProfilePhotoPath = "/img/user_def_icon.png"
        };
        var result = await _userManager.CreateAsync(user, Input.Password);

        if (result.Succeeded)
        {
            _logger.LogInformation("User created a new account with password");

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                null,
                new { userId = user.Id, code },
                Request.Scheme);

            await _emailSender.SendMailAsync(Input.Email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            await _signInManager.SignInAsync(user, isPersistent: false);
            return LocalRedirect(returnUrl);
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return Page();
    }
}
