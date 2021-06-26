using System.Threading.Tasks;

namespace SGM.Domain.Interfaces.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
