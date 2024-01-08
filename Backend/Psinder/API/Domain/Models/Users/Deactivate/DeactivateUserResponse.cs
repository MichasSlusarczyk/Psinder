namespace Psinder.API.Domain.Models.Users;

public class DeactivateUserResponse
{
    public DeactivateUserResponse(DeactivateUserStatus dectivateAccountStatus)
    {
        Status = dectivateAccountStatus;
    }

    public DeactivateUserStatus Status { get; set; }
}
