using MediatR;

namespace Psinder.API.Auth.Models.Registrations;

public class RegistrationMediatr : IRequest<AuthResponse>
{
    public RegistrationRequest Request { get; set; }
}