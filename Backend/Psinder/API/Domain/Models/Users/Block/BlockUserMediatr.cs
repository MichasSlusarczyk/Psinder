using MediatR;

namespace Psinder.API.Domain.Models.Users;

public class BlockUserMediatr : IRequest<BlockUserResponse>
{
    public BlockUserRequest Request { get; set; }
}
