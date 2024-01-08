namespace Psinder.API.Auth.Models.Logins;

public enum SendPasswordReminderStatus
{
    AccountNotExist = 1,

    InvalidAccountStatus = 2,

    BadRequest = 3,

    Ok = 4
}
