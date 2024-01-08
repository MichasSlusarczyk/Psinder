using MediatR;
using Psinder.API.Auth.Models;
using Psinder.API.Auth.Models.Registrations;
using Psinder.API.Auth.Services.Registration;

namespace Psinder.API.Auth.Handlers;

public class RegisterRequestHandler : IRequestHandler<RegistrationMediatr, AuthResponse>
{
    private readonly IRegisterService _registerService;
    private readonly ILogger<RegisterRequestHandler> _logger;

    public RegisterRequestHandler(
        IRegisterService registerService,
        ILogger<RegisterRequestHandler> logger)
    {
        _registerService = registerService;
        _logger = logger;
    }

    public async Task<AuthResponse> Handle(RegistrationMediatr requestMediatr, CancellationToken cancellationToken)
    {
        try
        {
            var request = requestMediatr.Request;

            return await _registerService.Register(request, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
