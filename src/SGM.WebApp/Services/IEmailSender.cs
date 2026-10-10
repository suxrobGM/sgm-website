namespace SGM.WebApp.Services;

public interface IEmailSender
{
    /// <summary>Returns false instead of throwing when delivery fails.</summary>
    Task<bool> SendMailAsync(string receiverMail, string subject, string htmlBody);
}
