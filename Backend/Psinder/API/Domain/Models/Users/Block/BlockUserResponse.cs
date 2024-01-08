namespace Psinder.API.Domain.Models.Users;

public class BlockUserResponse
{
    public BlockUserResponse(BlockUserStatus blockAccountStatus)
    {
        Status = blockAccountStatus;
    }

    public BlockUserStatus Status { get; set; }
}
