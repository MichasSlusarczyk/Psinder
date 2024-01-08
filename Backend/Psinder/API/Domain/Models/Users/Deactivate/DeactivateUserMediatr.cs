using MediatR;

namespace Psinder.API.Domain.Models.Users;

public class DeactivateUserMediatr : IRequest<DeactivateUserResponse>
{
    public DeactivateUserRequest Request { get; set; }
}
