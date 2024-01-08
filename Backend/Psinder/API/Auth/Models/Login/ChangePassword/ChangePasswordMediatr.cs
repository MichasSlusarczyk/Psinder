using MediatR;

namespace Psinder.API.Auth.Models.Logins;

public class ChangePasswordMediatr : IRequest<ChangePasswordResponse>
{
    public ChangePasswordRequest Request { get; set; }
}
