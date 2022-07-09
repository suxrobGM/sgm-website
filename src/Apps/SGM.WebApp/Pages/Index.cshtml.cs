using System.ComponentModel.DataAnnotations;
using SGM.Application.Contracts.Services;
using SGM.Application.Options;

namespace SGM.WebApp.Pages;

public class IndexModel : PageModel
{
    private readonly ICaptchaService _captchaService;
    private readonly IEmailSender _emailSender;

    public IndexModel(
        ICaptchaService captchaService,
        IEmailSender emailSender,
        ILogger<IndexModel> logger,
        GoogleRecaptchaOptions recaptchaOptions)
    {
        if (string.IsNullOrEmpty(recaptchaOptions.SiteKey))
        {
            throw new ArgumentException("Captcha site key is an empty");
        }

        _captchaService = captchaService;
        _emailSender = emailSender;
        CaptchaSiteKey = recaptchaOptions.SiteKey;
    }

    public class EmailInputModel
    {
        [Required]
        public string Name { get; init; }

        [Required]
        public string Subject { get; init; }

        [Required, EmailAddress]
        public string Email { get; init; }

        [Required]
        public string Message { get; init; }
    }

    [BindProperty]
    public EmailInputModel EmailInput { get; init; }

    [TempData]
    public string EmailStatusMessage { get; set; }

    public string CaptchaSiteKey { get; }


    public async Task<IActionResult> OnPostSendEmailAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var captchaValue = HttpContext.Request.Form["g-Recaptcha-Response"].ToString();
        var validCaptcha = await _captchaService.VerifyCaptchaAsync(captchaValue);

        if (!validCaptcha)
        {
            EmailStatusMessage = "Error: invalid captcha";
            return RedirectToPage("", "", "email");
        }

        var message = @$"<p><b>{EmailInput.Name}</b> - {EmailInput.Email}</p>
                         <p>{EmailInput.Message}</p>";
        var sentMail = await _emailSender.SendMailAsync("suxrobgm@gmail.com", EmailInput.Subject, message);

        EmailStatusMessage = sentMail ? "Your message has been sent successfully" : "Error: could not send email";
        return RedirectToPage("", "", "email");
    }
    
    public IActionResult OnGetCv()
    {
        return File("/cv.pdf", "application/pdf");
    }
}
