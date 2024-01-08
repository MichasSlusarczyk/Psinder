namespace Psinder.API.Auth.Models.Logins;

public class ResetPasswordResponse
{
    public ResetPasswordResponse(
        ResetPasswordStatus registrationStatus, 
        string? errorMessage = null)
    {
        Status = registrationStatus;
        ErrorMessage = errorMessage;
    }

    public ResetPasswordStatus Status { get; set; }
    public string? ErrorMessage { get; set; }
}
