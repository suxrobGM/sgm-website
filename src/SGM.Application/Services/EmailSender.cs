using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using SGM.Application.Services;
using SGM.Application.Options;

namespace SGM.Application.Services;

public sealed class EmailSender : IEmailSender
{
    private readonly EmailSenderOptions _options;
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(
        EmailSenderOptions options,
        ILogger<EmailSender> logger)
    {
        this._options = options ?? throw new ArgumentNullException(nameof(options));
        this._logger = logger;

        if (string.IsNullOrEmpty(options.SenderName))
            throw new ArgumentException("SenderName is a empty string");

        if (string.IsNullOrEmpty(options.SenderMail))
            throw new ArgumentException("SenderMail is a empty string");

        if (string.IsNullOrEmpty(options.Host))
            throw new ArgumentException("Host is a empty string");

        if (string.IsNullOrEmpty(options.UserName))
            throw new ArgumentException("UserName is a empty string");

        if (string.IsNullOrEmpty(options.Password))
            throw new ArgumentException("Password is a empty string");
    }
    
    public async Task<bool> SendMailAsync(string receiverMail, string subject, string htmlBody)
    {
        if (string.IsNullOrEmpty(receiverMail))
            throw new ArgumentNullException(nameof(receiverMail));
            
        if (string.IsNullOrEmpty(subject))
            throw new ArgumentNullException(nameof(subject));
            
        if (string.IsNullOrEmpty(htmlBody))
            throw new ArgumentNullException(nameof(htmlBody));

        try
        {
            var from = new MailAddress(_options.SenderMail!, _options.SenderName);
            using var mailMessage = new MailMessage(from, new MailAddress(receiverMail))
            {
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true,
                Priority = MailPriority.High,
                Headers = { },
                DeliveryNotificationOptions = DeliveryNotificationOptions.Delay |
                                              DeliveryNotificationOptions.OnFailure |
                                              DeliveryNotificationOptions.OnSuccess
            };

            using var smtpClient = new SmtpClient(_options.Host, _options.Port)
            {
                Port = _options.Port,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Host = _options.Host!,
                EnableSsl = true,
                Timeout = 5000,
                Credentials = new NetworkCredential(_options.UserName, _options.Password)
            };
            
            await smtpClient.SendMailAsync(mailMessage);
            _logger?.LogInformation("Email has been sent to {Mail}, subject: \'{Subject}\'", receiverMail, subject);
            return true;
        }
        catch (Exception ex)
        {
            _logger?.LogWarning(
                "Could not send email to {Mail}, subject: \'{Subject}\'. \nThrown exception: {Exception}", 
                receiverMail, subject, ex.ToString());
            return false;
        }
    }
}
