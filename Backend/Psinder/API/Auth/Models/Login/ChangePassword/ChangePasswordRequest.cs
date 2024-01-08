namespace Psinder.API.Auth.Models.Logins;

public class ChangePasswordRequest
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public string RepeatNewPassword { get; set; }
}
