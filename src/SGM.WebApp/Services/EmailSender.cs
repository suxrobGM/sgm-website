using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
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
        if (string.IsNullOrEmpty(receiverMail))
        {
            throw new ArgumentNullException(nameof(receiverMail));
        }

        if (string.IsNullOrEmpty(subject))
        {
            throw new ArgumentNullException(nameof(subject));
        }

        if (string.IsNullOrEmpty(htmlBody))
        {
            throw new ArgumentNullException(nameof(htmlBody));
        }

        try
        {
            var from = new MailAddress(_options.SenderMail!, _options.SenderName);
            using var mailMessage = new MailMessage(from, new MailAddress(receiverMail));
            mailMessage.Subject = subject;
            mailMessage.Body = htmlBody;
            mailMessage.IsBodyHtml = true;
            mailMessage.Priority = MailPriority.High;
            mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.Delay |
                                                      DeliveryNotificationOptions.OnFailure |
                                                      DeliveryNotificationOptions.OnSuccess;

            using var smtpClient = new SmtpClient(_options.Host, _options.Port);
            smtpClient.Port = _options.Port;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Host = _options.Host!;
            smtpClient.EnableSsl = true;
            smtpClient.Timeout = 5000;
            smtpClient.Credentials = new NetworkCredential(_options.UserName, _options.Password);

            await smtpClient.SendMailAsync(mailMessage);
            logger.LogInformation("Email has been sent to {Mail}, subject: \'{Subject}\'", receiverMail, subject);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogWarning(
                "Could not send email to {Mail}, subject: \'{Subject}\'. \nThrown exception: {Exception}",
                receiverMail, subject, ex.ToString());
            return false;
        }
    }
}
