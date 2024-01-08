using MediatR;
using Psinder.API.Auth.Models;
using Psinder.API.Auth.Models.Logins;
using Psinder.API.Auth.Services.Facebook;
using Psinder.API.Auth.Services.Login;
using Psinder.API.Auth.Services.Registration;

namespace Psinder.API.Auth.Handlers;

public class LoginFacebookRequestHandler : IRequestHandler<LoginFacebookMediatr, AuthResponse>
{
    private readonly IFacebookService _facebookService;
    private readonly ILoginService _loginService;
    private readonly IRegisterService _registerService;
    private readonly ILogger<LoginFacebookRequestHandler> _logger;

    public LoginFacebookRequestHandler(
        IFacebookService facebookService,
        ILoginService loginService,
        IRegisterService registerService,
        ILogger<LoginFacebookRequestHandler> logger)
    {
        _facebookService = facebookService;
        _loginService = loginService;
        _registerService = registerService;
        _logger = logger;
    }

    public async Task<AuthResponse> Handle(LoginFacebookMediatr requestMediatr, CancellationToken cancellationToken)
    {
        try
        {
            var request = requestMediatr.Request;

            try
            {
                if(!await _facebookService.VerifyAsync(request.FacebookToken, cancellationToken))
                {
                    return new AuthResponse(AuthStatus.InvalidToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Invalid Facebook token.");

                return new AuthResponse(AuthStatus.InvalidToken);
            }

            var email = await _facebookService.GetDataAsync(request.FacebookToken, cancellationToken);

            var result = await _loginService.Login(email, cancellationToken);

            if(result.Status == AuthStatus.AccountNotExist)
            {
                return await _registerService.RegisterWithService(email, cancellationToken);
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