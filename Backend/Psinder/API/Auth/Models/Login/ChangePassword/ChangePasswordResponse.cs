namespace Psinder.API.Auth.Models.Logins;

public class ChangePasswordResponse
{
    public ChangePasswordResponse(ChangePasswordStatus registrationStatus, string? errorMessage = null)
    {
        Status = registrationStatus;
        ErrorMessage = errorMessage;
    }

    public ChangePasswordStatus Status { get; set; }
    public string? ErrorMessage { get; set; }
}
