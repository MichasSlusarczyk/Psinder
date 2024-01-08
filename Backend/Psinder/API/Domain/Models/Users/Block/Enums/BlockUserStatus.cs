namespace Psinder.API.Domain.Models.Users;

public enum BlockUserStatus
{
    InvalidRequest = 1,

    Ok,

    AccessDenied,

    AlreadyBlocked
}
