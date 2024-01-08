namespace Psinder.API.Auth.Models.Logins;

public class ResetPasswordRequest
{
    public string RemindPasswordToken { get; set; }
    public string NewPassword { get; set; }
    public string RepeatNewPassword { get; set; }
}