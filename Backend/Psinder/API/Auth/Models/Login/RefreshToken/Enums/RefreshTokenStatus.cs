namespace Psinder.API.Auth.Models.Logins;

public enum RefreshTokenStatus
{
    InvalidToken = 1,

    AccessDenied,

    ExpiredToken,

    InvalidAccountStatus,

    Ok,

    AccountNotExist
}
