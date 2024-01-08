namespace Psinder.API.Auth.Models;

public enum AuthStatus
{
    Ok = 1,

    AccountNotExist,

    Inactive,

    Blocked,

    LoginInvalidPassword,

    Unconfirmed,

    InvalidRequest,

    RecaptchaFailed,

    InvalidToken,

    RegistrationInvalidPassword,

    EmailArleadyUsed,

    InvalidEmail,

    LoginArleadyUsed
}
