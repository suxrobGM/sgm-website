using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;
using System.Security;
using System.Threading.Tasks;

namespace SuxrobGM_Website.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Task.Run(() =>
            {
                string from = "DedSec94@mail.ru";
                using (var mm = new MailMessage(from, email))
                {
                    mm.Subject = subject;
                    mm.Body = htmlMessage;
                    mm.IsBodyHtml = true;

                    using (var sc = new SmtpClient("smtp.mail.ru", 25))
                    {
                        sc.EnableSsl = true;
                        sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                        sc.UseDefaultCredentials = false;
                        sc.Credentials = new NetworkCredential(from, GetS());
                        sc.Send(mm);
                    };
                };                           
            });
        }

        private SecureString GetS()
        {
            var secureString = new SecureString();
            secureString.AppendChar('s');
            secureString.AppendChar('u');
            secureString.AppendChar('x');
            secureString.AppendChar('r');
            secureString.AppendChar('o');
            secureString.AppendChar('b');
            secureString.AppendChar('b');
            secureString.AppendChar('e');
            secureString.AppendChar('k');
            return secureString;
        }
    }
}
