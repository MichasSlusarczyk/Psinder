namespace Psinder.API.Domain.Models.Users;

public enum AddShelterWithUsersResponseStatus
{
    EmailArleadyUsed = 2,

    InvalidEmail = 3,

    LoginArleadyUsed = 4,

    Ok = 5
}