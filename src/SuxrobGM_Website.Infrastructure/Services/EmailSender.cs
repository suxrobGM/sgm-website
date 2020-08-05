using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SuxrobGM_Website.Core.Interfaces.Services;

namespace SuxrobGM_Website.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            const string username = "noreply@suxrobgm.net";
            var password = _config.GetSection("EmailPassword").Value;

            using var mailMessage = new MailMessage(username, email)
            {
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            using var smtpClient = new SmtpClient("mail.hosting.reg.ru", 587)
            {
                EnableSsl = false,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(username, password)
            };

            // ReSharper disable once AsyncConverter.AsyncAwaitMayBeElidedHighlighting
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
