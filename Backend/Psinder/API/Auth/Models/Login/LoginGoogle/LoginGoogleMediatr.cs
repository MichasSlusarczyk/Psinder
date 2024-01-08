using MediatR;

namespace Psinder.API.Auth.Models.Logins;

public class LoginGoogleMediatr : IRequest<AuthResponse>
{
    public LoginGoogleRequest Request { get; set; }
}
