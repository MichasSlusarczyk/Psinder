namespace Psinder.API.Auth.Models.Logins;

public enum ChangePasswordStatus
{
    InvalidRequest = 1,

    InvalidPassword,

    InvalidNewPassword,

    AccessDenied,

    SamePassword,

    NewPasswordNotMatch,

    InvalidAccountStatus,

    Ok
}
