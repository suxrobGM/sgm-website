using System;
using System.Threading.Tasks;

namespace SGM.Domain.Interfaces.Services
{
    /// <summary>
    /// Email sender service
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Sends email to specified <paramref name="email"/> address.
        /// </summary>
        /// <param name="email">Receiver email address</param>
        /// <param name="subject">Email subject</param>
        /// <param name="htmlMessage">Email body formatted in html</param>
        /// <exception cref="ArgumentNullException">Throws if anyone arguments <paramref name="email"/>, <paramref name="subject"/> or <paramref name="htmlMessage"/> is null</exception>
        /// <exception cref="InvalidOperationException">Throws if could not load email settings from configuration file</exception>
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
