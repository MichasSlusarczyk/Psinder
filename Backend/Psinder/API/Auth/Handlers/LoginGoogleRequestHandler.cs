using Google.Apis.Auth;
using MediatR;
using Psinder.API.Auth.Models;
using Psinder.API.Auth.Models.Logins;
using Psinder.API.Auth.Services.Login;
using Psinder.API.Auth.Services.Registration;

namespace Psinder.API.Auth.Handlers;

public class LoginGoogleRequestHandler : IRequestHandler<LoginGoogleMediatr, AuthResponse>
{
    private readonly ILoginService _loginService;
    private readonly IRegisterService _registerService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<LoginGoogleRequestHandler> _logger;

    public LoginGoogleRequestHandler(
        ILoginService loginService,
        IRegisterService registerService,
        IConfiguration configuration,
        ILogger<LoginGoogleRequestHandler> logger)
    {
        _loginService = loginService;
        _registerService = registerService;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<AuthResponse> Handle(LoginGoogleMediatr requestMediatr, CancellationToken cancellationToken)
    {
        try
        {
            var request = requestMediatr.Request;

            var googleClientId = _configuration.GetSection("Google:GoogleClientId").Value
                ?? throw new Exception("Configuration value not found: Google:GoogleClientId");

            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { googleClientId }
            };

            GoogleJsonWebSignature.Payload payload;

            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(request.GoogleToken, settings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Invalid Google token.");

                return new AuthResponse(AuthStatus.InvalidToken);
            }

            var result = await _loginService.Login(payload.Email, cancellationToken);

            if (result.Status == AuthStatus.AccountNotExist)
            {
                return await _registerService.RegisterWithService(payload.Email, cancellationToken);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}