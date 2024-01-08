namespace Psinder.API.Auth.Models.Logins;

public enum ResetPasswordStatus
{
    InvalidToken = 1,

    InvalidRequest = 2,

    InvalidNewPassword = 3,

    SamePassword = 4,

    NewPasswordNotMatch = 5,

    InvalidAccountStatus = 6,

    Ok = 7
}
