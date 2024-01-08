namespace Psinder.API.Auth.Models.Logins;

public class RemindPasswordResponse
{
    public RemindPasswordResponse(
        RemindPasswordStatus registrationStatus, 
        string? errorMessage = null,
        string? login = null)
    {
        Status = registrationStatus;
        ErrorMessage = errorMessage;
        Login = login;
    }

    public RemindPasswordStatus Status { get; set; }
    public string? ErrorMessage { get; set; }
    public string? Login { get; set; }
}
