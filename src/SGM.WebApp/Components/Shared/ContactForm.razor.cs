using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Components;
using SGM.WebApp.Services;

namespace SGM.WebApp.Components.Shared;

public partial class ContactForm
{
    [Inject]
    private IEmailSender EmailSender { get; set; } = null!;

    [Inject]
    private ICaptchaService CaptchaService { get; set; } = null!;

    [Parameter]
    public string CaptchaSiteKey { get; set; } = string.Empty;

    [SupplyParameterFromForm(FormName = "contact")]
    private EmailInputModel? EmailInput { get; set; }

    private string? StatusMessage { get; set; }
    private bool IsError { get; set; }

    protected override void OnInitialized() => EmailInput ??= new EmailInputModel();

    private async Task HandleSubmit()
    {
        var input = EmailInput!;

        if (string.IsNullOrEmpty(input.RecaptchaToken) ||
            !await CaptchaService.VerifyCaptchaAsync(input.RecaptchaToken))
        {
            SetStatus("Error: failed reCAPTCHA check. Please try again.", isError: true);
            return;
        }

        // Visitor input goes into an HTML email body, so encode it.
        var message = $"""
                       <p><b>{WebUtility.HtmlEncode(input.Name)}</b> - {WebUtility.HtmlEncode(input.Email)}</p>
                       <p>{WebUtility.HtmlEncode(input.Message)}</p>
                       """;

        var sent = await EmailSender.SendMailAsync("suxrobgm@gmail.com", input.Subject!, message);

        if (!sent)
        {
            SetStatus("Error: could not send email", isError: true);
            return;
        }

        SetStatus("Your message has been sent successfully", isError: false);
        EmailInput = new EmailInputModel();
    }

    private void SetStatus(string message, bool isError)
    {
        StatusMessage = message;
        IsError = isError;
        // Tokens are single use; the next submit fetches a fresh one.
        EmailInput!.RecaptchaToken = null;
    }

    public class EmailInputModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        public string? Subject { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Message is required")]
        public string? Message { get; set; }

        public string? RecaptchaToken { get; set; }
    }
}
