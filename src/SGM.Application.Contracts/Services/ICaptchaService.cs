namespace SGM.Application.Contracts.Services;

public interface ICaptchaService
{
    Task<bool> VerifyCaptchaAsync(string captchaValue);
}
