using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SGM.WebApp.Services;

namespace SGM.WebApp.Components.Shared;

public partial class ContactForm
{
    [Inject]
    private IEmailSender EmailSender { get; set; } = null!;

    [Inject]
    private ICaptchaService CaptchaService { get; set; } = null!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = null!;

    [Parameter]
    public string CaptchaSiteKey { get; set; } = string.Empty;

    private EmailInputModel EmailInput { get; set; } = new();
    private string? StatusMessage { get; set; }
    private string RecaptchaToken { get; set; } = string.Empty;
    private bool IsSubmitting { get; set; }

    private async Task HandleSubmit()
    {
        IsSubmitting = true;
        StatusMessage = null;

        try
        {
            // Get reCAPTCHA token from JavaScript
            RecaptchaToken = await JSRuntime.InvokeAsync<string>("getRecaptchaToken", CaptchaSiteKey);

            if (string.IsNullOrEmpty(RecaptchaToken))
            {
                StatusMessage = "Error: Could not verify reCAPTCHA";
                return;
            }

            var isHuman = await CaptchaService.VerifyCaptchaAsync(RecaptchaToken);

            if (!isHuman)
            {
                StatusMessage = "Error: failed reCAPTCHA check";
                return;
            }

            var message = $"""
                           <p><b>{EmailInput.Name}</b> - {EmailInput.Email}</p>
                           <p>{EmailInput.Message}</p>
                           """;

            var sentMail = await EmailSender.SendMailAsync("suxrobgm@gmail.com", EmailInput.Subject!, message);

            StatusMessage = sentMail ? "Your message has been sent successfully" : "Error: could not send email";

            if (sentMail)
            {
                EmailInput = new EmailInputModel();
            }
        }
        finally
        {
            IsSubmitting = false;
        }
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
    }
}
