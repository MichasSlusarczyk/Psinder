using MediatR;

namespace Psinder.API.Domain.Models.Users;

public class UpdateUserMediatr : IRequest<UpdateUserResponse>
{
    public UpdateUserRequest Request { get; set; }
}
