using Psinder.API.Auth.Models.Logins;
using Psinder.API.Common;
using MediatR;
using System.Text;
using Psinder.DB.Auth.Entities;
using Psinder.DB.Auth.Models;
using Psinder.DB.Auth.Repositories.Logins;
using Psinder.API.Domain.Models.Users;
using Psinder.DB.Domain.Entities;

namespace Psinder.API.Auth.Handlers;

public class ChangePasswordRequestHandler : IRequestHandler<ChangePasswordMediatr, ChangePasswordResponse>
{
    private readonly IIdentityService _identityService;
    private readonly ILoginRepository _loginRepository;
    private readonly IPasswordValidationService _passwordValidationService;
    private readonly IHashService _hashService;
    private readonly ILogger<ChangePasswordRequestHandler> _logger;

    public ChangePasswordRequestHandler(
        IIdentityService identityService,
        ILoginRepository loginRepository,
        IPasswordValidationService passwordValidationService,
        IHashService hashService,
        ILogger<ChangePasswordRequestHandler> logger)
    {
        _identityService = identityService;
        _loginRepository = loginRepository;
        _passwordValidationService = passwordValidationService;
        _hashService = hashService;
        _logger = logger;
    }

    public async Task<ChangePasswordResponse> Handle(ChangePasswordMediatr requestMediatr, CancellationToken cancellationToken)
    {
        try
        {
            var request = requestMediatr.Request;

            var loggedUserId = _identityService.CurrentUserId
                ?? throw new Exception("User ID in token not found.");

            var passwordData = await _loginRepository.GetPasswordData(loggedUserId, cancellationToken)
                ?? throw new Exception($"Password data for user ID: {loggedUserId} not found.");

            var validationResult = await ValidateChangePasswordAttempt(request, passwordData, loggedUserId, cancellationToken);

            if (validationResult.Status != ChangePasswordStatus.Ok)
            {
                return validationResult;
            }

            _hashService.GeneratePasswordHash(Encoding.UTF8.GetBytes(request.NewPassword), out byte[] passwordSalt, out byte[] passwordHash);

            await _loginRepository.ChangePassword(passwordData.Id, passwordHash, passwordSalt, cancellationToken);

            return new ChangePasswordResponse(ChangePasswordStatus.Ok);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    private async Task<ChangePasswordResponse> ValidateChangePasswordAttempt(ChangePasswordRequest request, PasswordDto passwordData, long userId, CancellationToken cancellationToken)
    {
        if (!_identityService.VerifyAccess(userId))
        {
            return new ChangePasswordResponse(ChangePasswordStatus.AccessDenied);
        }

        if (string.IsNullOrEmpty(request.OldPassword) || 
            string.IsNullOrEmpty(request.NewPassword) ||
            string.IsNullOrEmpty(request.RepeatNewPassword))
        {
            return new ChangePasswordResponse(ChangePasswordStatus.InvalidRequest);
        }

        if (passwordData.AccountStatus != AccountStatuses.Active)
        {
            return new ChangePasswordResponse(ChangePasswordStatus.InvalidAccountStatus);
        }

        if (request.OldPassword.Equals(request.NewPassword))
        {
            return new ChangePasswordResponse(ChangePasswordStatus.SamePassword);
        }

        if (!request.NewPassword.Equals(request.RepeatNewPassword))
        {
            return new ChangePasswordResponse(ChangePasswordStatus.NewPasswordNotMatch);
        }

        var passwordSyntaxValidationResult = await _passwordValidationService.ValidateLoginPassword(request.NewPassword, cancellationToken);

        if (!passwordSyntaxValidationResult.Result)
        {
            return new ChangePasswordResponse(ChangePasswordStatus.InvalidNewPassword, passwordSyntaxValidationResult.ErrorMessage);
        }

        if (!_hashService.VerifyPasswordHash(Encoding.UTF8.GetBytes(request.OldPassword), passwordData.PasswordHash, passwordData.Salt))
        {
            return new ChangePasswordResponse(ChangePasswordStatus.InvalidPassword);
        }

        return new ChangePasswordResponse(ChangePasswordStatus.Ok);
    }
}