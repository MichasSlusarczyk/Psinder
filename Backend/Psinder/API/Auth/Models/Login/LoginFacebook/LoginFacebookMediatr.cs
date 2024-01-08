using MediatR;

namespace Psinder.API.Auth.Models.Logins;

public class LoginFacebookMediatr : IRequest<AuthResponse>
{
    public LoginFacebookRequest Request { get; set; }
}
