using MediatR;
using Microsoft.IdentityModel.Tokens;
using Psinder.API.Auth.Models.Logins;
using Psinder.DB.Auth.Entities;
using Psinder.DB.Auth.Models;
using Psinder.DB.Auth.Repositories.Logins;

namespace Psinder.API.Auth.Handlers;

public class RemindPasswordRequestHandler : IRequestHandler<RemindPasswordMediatr, RemindPasswordResponse>
{
    private readonly ILoginRepository _loginRepository;
    private readonly ILogger<RemindPasswordRequestHandler> _logger;

    public RemindPasswordRequestHandler(
        ILoginRepository loginRepository,
        ILogger<RemindPasswordRequestHandler> logger)
    {
        _loginRepository = loginRepository;
        _logger = logger;
    }

    public async Task<RemindPasswordResponse> Handle(RemindPasswordMediatr requestMediatr, CancellationToken cancellationToken)
    {
        try
        {
            var request = requestMediatr.Request;

            var remindPasswordData = await _loginRepository.GetRemindPasswordData(request.RemindPasswordToken, cancellationToken);

            var validationResult = ValidateRemindPasswordData(remindPasswordData, request.RemindPasswordToken);

            if (validationResult != RemindPasswordStatus.Ok)
            {
                return new RemindPasswordResponse(validationResult);
            }

            return new RemindPasswordResponse(RemindPasswordStatus.Ok, null, remindPasswordData.Login);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex?.Message);
            throw;
        }
    }

    private RemindPasswordStatus ValidateRemindPasswordData(RemindPasswordDto remindPasswordData, string remindPasswordToken)
    {
        if (remindPasswordToken.IsNullOrEmpty())
        {
            return RemindPasswordStatus.BadRequest;
        }

        if (remindPasswordData.ResetPasswordToken == null || remindPasswordData.ResetPasswordToken.Value != remindPasswordToken)
        {
            return RemindPasswordStatus.InvalidToken;
        }

        if (remindPasswordData.AccountStatusId != (byte)AccountStatuses.Active)
        {
            return RemindPasswordStatus.InvalidAccountStatus;
        }

        if (DateTime.Compare(remindPasswordData.ResetPasswordToken.ExpirationDate, DateTime.UtcNow) < 0)
        {
            return RemindPasswordStatus.ExpiredToken;
        }

        return RemindPasswordStatus.Ok;
    }
}
