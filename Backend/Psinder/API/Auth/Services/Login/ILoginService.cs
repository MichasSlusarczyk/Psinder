using Psinder.API.Auth.Models.Logins;
using Psinder.API.Auth.Models;

namespace Psinder.API.Auth.Services.Login;

public interface ILoginService
{
    Task<AuthResponse> Login(string email, CancellationToken cancellationToken, LoginRequest? loginRequest = null);
}
