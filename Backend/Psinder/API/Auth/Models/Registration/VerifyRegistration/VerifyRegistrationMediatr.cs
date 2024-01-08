using MediatR;

namespace Psinder.API.Auth.Models.Registrations;

public class VerifyRegistrationMediatr : IRequest<VerifyRegistrationResponse>
{
    public VerifyRegistrationRequest Request { get; set; }
}
