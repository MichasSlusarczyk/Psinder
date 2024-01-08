using Microsoft.IdentityModel.Tokens;
using Psinder.API.Auth.Common;
using Psinder.API.Auth.Models;
using Psinder.API.Auth.Models.Logins;
using Psinder.API.Auth.Services.Recaptcha;
using Psinder.API.Common;
using Psinder.DB.Auth.Entities;
using Psinder.DB.Auth.Models;
using Psinder.DB.Auth.Repositories.Logins;
using Psinder.DB.Common.Entities;
using Psinder.DB.Common.Services;
using System.Text;

namespace Psinder.API.Auth.Services.Login;

public class LoginService : ILoginService
{
    private readonly ILoginRepository _loginRepository;
    private readonly ICacheService _cacheService;
    private readonly IHashService _hashService;
    private readonly ITokenService _tokenService;
    private readonly IRecaptchaService _recaptchaService;

    public LoginService(
        ILoginRepository loginRepository,
        ICacheService cacheService,
        IHashService hashService,
        ITokenService tokenService,
        IRecaptchaService recaptchaService)
    {
        _loginRepository = loginRepository;
        _cacheService = cacheService;
        _hashService = hashService;
        _tokenService = tokenService;
        _recaptchaService = recaptchaService;
    }

    public async Task<AuthResponse> Login(string email, CancellationToken cancellationToken, LoginRequest? loginRequest = null)
    {
        var loginData = await _loginRepository.GetLoginValidationData(email, cancellationToken);

        var validationResult = await ValidateLoginAttempt(loginData, cancellationToken, loginRequest);

        if (validationResult != AuthStatus.Ok)
        {
            return new AuthResponse(validationResult);
        }

        var loginToken = await _tokenService.GenerateLoginToken(loginData.UserId, loginData.Id, Enum.GetName(typeof(Role), loginData.RoleId));
        var refreshToken = await _tokenService.GenerateRefreshToken();

        await _loginRepository.Login(loginData.Id, refreshToken, cancellationToken);

        return new AuthResponse(AuthStatus.Ok, loginToken, refreshToken.Value, loginData.UserId);
    }

    private async Task<AuthStatus> ValidateLoginAttempt(LoginValidationDto? loginData, CancellationToken cancellationToken, LoginRequest? request = null)
    {
        if (loginData == null)
        {
            return AuthStatus.AccountNotExist;
        }

        if (request != null)
        {
            if (request.Password.IsNullOrEmpty() || request.Login.IsNullOrEmpty() || request.CaptchaResponse.IsNullOrEmpty())
            {
                return AuthStatus.InvalidRequest;
            }

            if (!_hashService.VerifyPasswordHash(Encoding.UTF8.GetBytes(request.Password), loginData.PasswordHash, loginData.Salt))
            {
                await _loginRepository.UpdateLoginAttemptsCount(loginData.Id, ++loginData.LoginAttempts, cancellationToken);

                return AuthStatus.LoginInvalidPassword;
            }

            if (!await _recaptchaService.VerifyAsync(request.CaptchaResponse))
            {
                return AuthStatus.RecaptchaFailed;
            }
        }

        if (loginData == null)
        {
            return AuthStatus.AccountNotExist;
        }

        var maxLoginAttemptsCount = await _cacheService.GetConfigurationIntValueByItem(
                ConfigurationCategory.Login,
                ConfigurationItem.BadLoginAtteptsCount,
                cancellationToken);

        if (loginData.LoginAttempts > maxLoginAttemptsCount)
        {
            await _loginRepository.UpdateAccountStatus(loginData.Id, AccountStatuses.Blocked, cancellationToken);

            return AuthStatus.Blocked;
        }

        if (loginData.AccountStatusId == (byte)AccountStatuses.Blocked)
        {
            return AuthStatus.Blocked;
        }
        else if (loginData.AccountStatusId == (byte)AccountStatuses.Inactive)
        {
            return AuthStatus.Inactive;
        }
        else if (loginData.AccountStatusId == (byte)AccountStatuses.Unconfirmed)
        {
            return AuthStatus.Unconfirmed;
        }

        return AuthStatus.Ok;
    }
}
