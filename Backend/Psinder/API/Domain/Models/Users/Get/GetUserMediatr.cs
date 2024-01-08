using MediatR;

namespace Psinder.API.Domain.Models.Users;

public class GetUserMediatr : IRequest<GetUserResponse>
{
    public GetUserRequest Request { get; set; }
}
