using MediatR;
using Psinder.API.Auth.Models.Registrations;
using Psinder.DB.Auth.Entities;
using Psinder.DB.Auth.Models;
using Psinder.DB.Auth.Repositories.Logins;

namespace Psinder.API.Auth.Handlers;

public class VerifyRegistrationRequestHandler : IRequestHandler<VerifyRegistrationMediatr, VerifyRegistrationResponse>
{
    private readonly ILoginRepository _loginRepository;
    private readonly ILogger<VerifyRegistrationRequestHandler> _logger;

    public VerifyRegistrationRequestHandler(
        ILoginRepository loginRepository, 
        ILogger<VerifyRegistrationRequestHandler> logger)
    {
        _loginRepository = loginRepository;
        _logger = logger;
    }

    public async Task<VerifyRegistrationResponse> Handle(VerifyRegistrationMediatr requestMediatr, CancellationToken cancellationToken)
    {
        try
        {
            var request = requestMediatr.Request;

            var verifyRegistrationData = await _loginRepository.GetVerifyRegistrationData(request.RegisterVerificationToken, cancellationToken);

            var validationResult = ValidateVerifyRegistrationData(verifyRegistrationData);

            if (validationResult != VerifyRegistrationStatus.Ok)
            {
                return new VerifyRegistrationResponse(validationResult);
            }

            await _loginRepository.VerifyRegistration(verifyRegistrationData.Id.Value, cancellationToken);

            return new VerifyRegistrationResponse(VerifyRegistrationStatus.Ok);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    private static VerifyRegistrationStatus ValidateVerifyRegistrationData(VerifyRegistrationDto verifyRegistrationData)
    {
        if (verifyRegistrationData == null)
        {
            return VerifyRegistrationStatus.InvalidToken;
        }

        if (verifyRegistrationData.AccountStatusId != (byte)AccountStatuses.Unconfirmed)
        {
            return VerifyRegistrationStatus.InvalidAccountStatus;
        }

        if (DateTime.Compare(verifyRegistrationData.RegisterVerificationToken.ExpirationDate, DateTime.UtcNow) < 0)
        {
            return VerifyRegistrationStatus.ExpiredToken;
        }

        return VerifyRegistrationStatus.Ok;
    }
}
