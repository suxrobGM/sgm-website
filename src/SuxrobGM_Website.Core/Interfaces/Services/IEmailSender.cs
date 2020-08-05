using System.Threading.Tasks;

namespace SuxrobGM_Website.Core.Interfaces.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
