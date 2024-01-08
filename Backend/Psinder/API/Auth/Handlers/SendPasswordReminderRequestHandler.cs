using MediatR;
using Microsoft.IdentityModel.Tokens;
using Psinder.API.Auth.Common;
using Psinder.API.Auth.Models.Logins;
using Psinder.API.Common;
using Psinder.DB.Auth.Entities;
using Psinder.DB.Auth.Models;
using Psinder.DB.Auth.Repositories.Logins;
using System.Web;

namespace Psinder.API.Auth.Handlers;

public class SendPasswordReminderRequestHandler : IRequestHandler<SendPasswordReminderMediatr, SendPasswordReminderResponse>
{
    private readonly ITokenService _tokenService;
    private readonly ILoginRepository _loginRepository;
    private readonly IEmailSenderService _emailSenderService;
    private readonly ILogger<SendPasswordReminderRequestHandler> _logger;

    public SendPasswordReminderRequestHandler(
        ITokenService tokenService,
        ILoginRepository loginRepostory,
        IEmailSenderService emailSenderService,
        ILogger<SendPasswordReminderRequestHandler> logger)
    {
        _tokenService = tokenService;
        _loginRepository = loginRepostory;
        _emailSenderService = emailSenderService;
        _logger = logger;
    }

    public async Task<SendPasswordReminderResponse> Handle(SendPasswordReminderMediatr requestMediatr, CancellationToken cancellationToken)
    {
        try
        {
            var request = requestMediatr.Request;

            var loginData = await _loginRepository.GetLoginValidationData(request.Login, cancellationToken);

            var validationResult = ValidateData(request, loginData);

            if (validationResult != SendPasswordReminderStatus.Ok)
            {
                return new SendPasswordReminderResponse(validationResult);
            }

            var token = await _tokenService.GenerateResetPasswordToken();

            await _loginRepository.UpdateResetPasswordToken(loginData.Id, token, cancellationToken);

            var to = loginData.Login;
            var resetPasswordToken = HttpUtility.UrlEncode(token.Value);

            await _emailSenderService.SendPasswordReminderEmail(to, resetPasswordToken);

            return new SendPasswordReminderResponse(SendPasswordReminderStatus.Ok);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    private static SendPasswordReminderStatus ValidateData(
        SendPasswordReminderRequest request, 
        LoginValidationDto loginData)
    {
        if (request.Login.IsNullOrEmpty())
        {
            return SendPasswordReminderStatus.BadRequest;
        }

        if (loginData == null)
        {
            return SendPasswordReminderStatus.AccountNotExist;
        }

        if (loginData.AccountStatusId != (byte)AccountStatuses.Active)
        {
            return SendPasswordReminderStatus.InvalidAccountStatus;
        }

        return SendPasswordReminderStatus.Ok;
    }
}
