using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SGM.Domain.Interfaces.Services;
using SGM.Domain.Settings;

namespace SGM.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email));
            
            if (string.IsNullOrWhiteSpace(subject))
                throw new ArgumentNullException(nameof(subject));
            
            if (string.IsNullOrWhiteSpace(htmlMessage))
                throw new ArgumentNullException(nameof(htmlMessage));
            
            var emailSettings = _configuration.GetSection(nameof(EmailSettings))?.Get<EmailSettings>();

            if (emailSettings == default)
                throw new InvalidOperationException("Could not load email settings");

            using var mailMessage = new MailMessage(emailSettings.UserName, email)
            {
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            using var smtpClient = new SmtpClient(emailSettings.Host, emailSettings.Port)
            {
                EnableSsl = false,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(emailSettings.UserName, emailSettings.Password)
            };
            
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
