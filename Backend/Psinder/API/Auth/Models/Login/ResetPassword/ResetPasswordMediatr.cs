using MediatR;

namespace Psinder.API.Auth.Models.Logins;

public class ResetPasswordMediatr : IRequest<ResetPasswordResponse>
{
    public ResetPasswordRequest Request { get; set; }
}
