using MediatR;
using Psinder.API.Auth.Models;
using Psinder.API.Auth.Models.Logins;
using Psinder.API.Auth.Services.Login;
using Psinder.DB.Auth.Repositories.Logins;

namespace Psinder.API.Auth.Handlers;

public class LoginRequestHandler : IRequestHandler<LoginMediatr, AuthResponse>
{
    private readonly ILoginService _loginService;
    private readonly ILogger<LoginRequestHandler> _logger;

    public LoginRequestHandler(
        ILoginService loginService,
        ILogger<LoginRequestHandler> logger)
    {
        _loginService = loginService;
        _logger = logger;
    }

    public async Task<AuthResponse> Handle(LoginMediatr requestMediatr, CancellationToken cancellationToken)
    {
        try
        {
            var request = requestMediatr.Request;

            return await _loginService.Login(request.Login, cancellationToken, request);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}