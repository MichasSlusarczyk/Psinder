namespace Psinder.API.Auth.Services.Recaptcha;

public interface IRecaptchaService
{
    Task<bool> VerifyAsync(string token);
}
