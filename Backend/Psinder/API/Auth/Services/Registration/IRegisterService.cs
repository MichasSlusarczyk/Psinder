using Psinder.API.Auth.Models.Registrations;
using Psinder.API.Auth.Models;

namespace Psinder.API.Auth.Services.Registration;

public interface IRegisterService
{
    Task<AuthResponse> Register(RegistrationRequest request, CancellationToken cancellationToken);

    Task<AuthResponse> RegisterWithService(string email, CancellationToken cancellationToken);
}