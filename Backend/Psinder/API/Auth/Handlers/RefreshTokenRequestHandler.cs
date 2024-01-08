using MediatR;
using Psinder.API.Auth.Common;
using Psinder.API.Auth.Models.Logins;
using Psinder.API.Common;
using Psinder.DB.Auth.Entities;
using Psinder.DB.Auth.Models;
using Psinder.DB.Auth.Repositories.Logins;

namespace Psinder.API.Auth.Handlers;

public class RefreshTokenRequestHandler : IRequestHandler<RefreshTokenMediatr, RefreshTokenResponse>
{
    private readonly ITokenService _tokenService;
    private readonly ILoginRepository _loginRepository;
    private readonly ILogger<RefreshTokenRequestHandler> _logger;

    public RefreshTokenRequestHandler(
        ITokenService tokenService,
        ILoginRepository loginRepository,
        ILogger<RefreshTokenRequestHandler> logger)
    {
        _tokenService = tokenService;
        _loginRepository = loginRepository;
        _logger = logger;
    }

    public async Task<RefreshTokenResponse> Handle(RefreshTokenMediatr requestMediatr, CancellationToken cancellationToken)
    {
        try
        {
            var request = requestMediatr.Request;

            var refreshTokenData = await _loginRepository.GetRefreshTokenData(request.RefreshToken, cancellationToken);

            var validationResult = ValidateRefreshTokenData(refreshTokenData, request);

            if (validationResult != RefreshTokenStatus.Ok)
            {
                return new RefreshTokenResponse(validationResult);
            }

            var loginToken = await _tokenService.GenerateLoginToken(refreshTokenData.UserId, refreshTokenData.LoginId, Enum.GetName(typeof(Role), refreshTokenData.RoleId));
            var refreshToken = await _tokenService.GenerateRefreshToken();

            await _loginRepository.RefreshToken(refreshTokenData.LoginId, refreshToken, cancellationToken);

            return new RefreshTokenResponse(RefreshTokenStatus.Ok, loginToken, refreshToken.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    private RefreshTokenStatus ValidateRefreshTokenData(RefreshTokenDto? refreshTokenData, RefreshTokenRequest request)
    {
        if (refreshTokenData == null)
        {
            return RefreshTokenStatus.AccountNotExist;
        }

        if (refreshTokenData.AccountStatusId != (byte)AccountStatuses.Active)
        {
            return RefreshTokenStatus.InvalidAccountStatus;
        }

        if (refreshTokenData.RefreshToken.Value != request.RefreshToken)
        {
            return RefreshTokenStatus.InvalidToken;
        }

        if (DateTime.Compare(refreshTokenData.RefreshToken.ExpirationDate, DateTime.UtcNow) < 0)
        {
            return RefreshTokenStatus.ExpiredToken;
        }

        return RefreshTokenStatus.Ok;
    }
}
