using MediatR;

namespace Psinder.API.Auth.Models.Logins;

public class RemindPasswordMediatr : IRequest<RemindPasswordResponse>
{
    public RemindPasswordRequest Request { get; set; }
}
