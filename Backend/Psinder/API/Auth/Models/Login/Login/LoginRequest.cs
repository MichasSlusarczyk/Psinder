namespace Psinder.API.Auth.Models.Logins;

public class LoginRequest
{
    public string Login { get; set; }
    public string Password { get; set; }

    public string CaptchaResponse { get; set; }
}
