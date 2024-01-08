namespace Psinder.API.Auth.Models.Logins;

public enum RemindPasswordStatus
{
    InvalidToken = 1,

    ExpiredToken,

    BadRequest,

    InvalidAccountStatus,

    Ok
}
