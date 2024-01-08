using MediatR;
using Psinder.API.Auth.Models.Logins;
using Psinder.API.Common;
using Psinder.DB.Auth.Entities;
using Psinder.DB.Auth.Models;
using Psinder.DB.Auth.Repositories.Logins;
using System.Text;

namespace Psinder.API.Auth.Handlers;

public class ResetPasswordRequestHandler : IRequestHandler<ResetPasswordMediatr, ResetPasswordResponse>
{
    private readonly ILoginRepository _loginRepository;
    private readonly IPasswordValidationService _passwordValidationService;
    private readonly IHashService _hashService;
    private readonly ILogger<ResetPasswordRequestHandler> _logger;

    public ResetPasswordRequestHandler(
        ILoginRepository loginRepository,
        IPasswordValidationService passwordValidationService,
        IHashService hashService,
        ILogger<ResetPasswordRequestHandler> logger)
    {
        _loginRepository = loginRepository;
        _passwordValidationService = passwordValidationService;
        _hashService = hashService;
        _logger = logger;
    }

    public async Task<ResetPasswordResponse> Handle(ResetPasswordMediatr requestMediatr, CancellationToken cancellationToken)
    {
        try
        {
            var request = requestMediatr.Request;

            var passwordData = await _loginRepository.GetResetPasswordData(request.RemindPasswordToken, cancellationToken);

            var validationResult = await ValidateChangePasswordAttempt(request, passwordData, cancellationToken);

            if (validationResult.Status != ResetPasswordStatus.Ok)
            {
                return validationResult;
            }

            _hashService.GeneratePasswordHash(Encoding.UTF8.GetBytes(request.NewPassword), out byte[] passwordSalt, out byte[] passwordHash);

            await _loginRepository.ResetPassword(passwordData.Id, passwordHash, passwordSalt, cancellationToken);

            return new ResetPasswordResponse(ResetPasswordStatus.Ok);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    private async Task<ResetPasswordResponse> ValidateChangePasswordAttempt(
        ResetPasswordRequest request,
        PasswordDto? passwordData, 
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.RemindPasswordToken) ||
            string.IsNullOrEmpty(request.NewPassword) ||
            string.IsNullOrEmpty(request.RepeatNewPassword))
        {
            return new ResetPasswordResponse(ResetPasswordStatus.InvalidRequest);
        }

        if (passwordData == null)
        {
            return new ResetPasswordResponse(ResetPasswordStatus.InvalidToken);
        }

        if (passwordData.AccountStatus != AccountStatuses.Active)
        {
            return new ResetPasswordResponse(ResetPasswordStatus.InvalidAccountStatus);
        }

        if (!request.NewPassword.Equals(request.RepeatNewPassword))
        {
            return new ResetPasswordResponse(ResetPasswordStatus.NewPasswordNotMatch);
        }

        var passwordSyntaxValidationResult = await _passwordValidationService.ValidateLoginPassword(request.NewPassword, cancellationToken);

        if (!passwordSyntaxValidationResult.Result)
        {
            return new ResetPasswordResponse(ResetPasswordStatus.InvalidNewPassword, passwordSyntaxValidationResult.ErrorMessage);
        }

        if (_hashService.VerifyPasswordHash(Encoding.UTF8.GetBytes(request.NewPassword), passwordData.PasswordHash, passwordData.Salt))
        {
            return new ResetPasswordResponse(ResetPasswordStatus.SamePassword);
        }

        return new ResetPasswordResponse(ResetPasswordStatus.Ok);
    }
}