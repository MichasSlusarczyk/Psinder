namespace Psinder.API.Auth.Models.Logins;

public class SendPasswordReminderResponse
{
    public SendPasswordReminderResponse(
        SendPasswordReminderStatus registrationStatus, 
        string? errorMessage = null,
        string? login = null)
    {
        Status = registrationStatus;
        ErrorMessage = errorMessage;
    }

    public SendPasswordReminderStatus Status { get; set; }
    public string? ErrorMessage { get; set; }
}
