using MediatR;

namespace Psinder.API.Auth.Models.Logins;

public class LoginMediatr : IRequest<AuthResponse>
{
    public LoginRequest Request { get; set; }
}
