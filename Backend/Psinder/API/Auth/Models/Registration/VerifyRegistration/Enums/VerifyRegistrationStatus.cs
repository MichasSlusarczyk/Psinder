namespace Psinder.API.Auth.Models.Registrations;

public enum VerifyRegistrationStatus
{
    InvalidToken = 1,

    ExpiredToken = 2,

    InvalidAccountStatus = 3,

    Ok = 4
}
