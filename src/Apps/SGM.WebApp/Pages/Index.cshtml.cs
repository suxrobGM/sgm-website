using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using SGM.Application.Options;
using SGM.Application.Services;

namespace SGM.WebApp.Pages;

public class IndexModel : PageModel
{
    private readonly ICaptchaService _captchaService;
    private readonly IEmailSender _emailSender;

    public IndexModel(
        ICaptchaService captchaService,
        IEmailSender emailSender,
        ILogger<IndexModel> logger,
        IOptions<GoogleRecaptchaOptions> options)
    {
        _captchaService = captchaService;
        _emailSender = emailSender;
        CaptchaSiteKey = options.Value.SiteKey;
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

        var token = Request.Form["recaptcha_token"];
        var isHuman = await _captchaService.VerifyCaptchaAsync(token);

        if (!isHuman)
        {
            EmailStatusMessage = "Error: failed reCAPTCHA check";
            return RedirectToPage("", "", "email");
        }

        var message = $"""
                       <p><b>{EmailInput.Name}</b> - {EmailInput.Email}</p>
                                                <p>{EmailInput.Message}</p>
                       """;
        var sentMail = await _emailSender.SendMailAsync("suxrobgm@gmail.com", EmailInput.Subject, message);

        EmailStatusMessage = sentMail ? "Your message has been sent successfully" : "Error: could not send email";
        return RedirectToPage("", "", "email");
    }
}
