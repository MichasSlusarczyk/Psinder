using MediatR;

namespace Psinder.API.Auth.Models.Logins;

public class RefreshTokenMediatr : IRequest<RefreshTokenResponse>
{
    public RefreshTokenRequest Request { get; set; }
}