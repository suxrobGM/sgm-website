using Microsoft.Extensions.Options;
using Resend;
using SGM.WebApp.Options;

namespace SGM.WebApp.Services;

public sealed class EmailSender(
    IOptions<EmailSenderOptions> options,
    ILogger<EmailSender> logger)
    : IEmailSender
{
    private readonly EmailSenderOptions _options = options.Value;

    public async Task<bool> SendMailAsync(string receiverMail, string subject, string htmlBody)
    {
        ArgumentException.ThrowIfNullOrEmpty(receiverMail);
        ArgumentException.ThrowIfNullOrEmpty(subject);
        ArgumentException.ThrowIfNullOrEmpty(htmlBody);

        try
        {
            var client = ResendClient.Create(_options.ApiKey!);
            var message = new EmailMessage
            {
                From = $"{_options.SenderName} <{_options.SenderMail}>",
                To = [receiverMail],
                Subject = subject,
                HtmlBody = htmlBody
            };

            await client.EmailSendAsync(message);
            logger.LogInformation("Email has been sent to {Mail}, subject: '{Subject}'", receiverMail, subject);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogWarning(
                "Could not send email to {Mail}, subject: '{Subject}'. \nThrown exception: {Exception}",
                receiverMail, subject, ex.ToString());
            return false;
        }
    }
}
