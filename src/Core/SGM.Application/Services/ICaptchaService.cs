namespace SGM.Application.Services;

public interface ICaptchaService
{
    Task<bool> VerifyCaptchaAsync(string captchaValue);
}
