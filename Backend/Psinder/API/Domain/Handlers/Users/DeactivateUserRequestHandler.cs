using MediatR;
using Psinder.API.Common;
using Psinder.API.Domain.Models.Users;
using Psinder.DB.Auth.Entities;
using Psinder.DB.Auth.Repositories.Logins;

namespace Psinder.API.Domain.Handlers;

public class DeactivateUserRequestHandler : IRequestHandler<DeactivateUserMediatr, DeactivateUserResponse>
{
    private readonly IIdentityService _identityService;
    private readonly ILoginRepository _loginRepository;
    private readonly ILogger<DeactivateUserRequestHandler> _logger;

    public DeactivateUserRequestHandler(
        IIdentityService identityService,
        ILoginRepository loginRepository,
        ILogger<DeactivateUserRequestHandler> logger)
    {
        _identityService = identityService;
        _loginRepository = loginRepository;
        _logger = logger;
    }

    public async Task<DeactivateUserResponse> Handle(DeactivateUserMediatr requestMediatr, CancellationToken cancellationToken)
    {
        try
        {
            var request = requestMediatr.Request;

            var validationResult = ValidateDectivateUserAttempt(request);

            if (validationResult.Status != DeactivateUserStatus.Ok)
            {
                return validationResult;
            }

            await _loginRepository.UpdateAccountStatus(request.Id, AccountStatuses.Inactive, cancellationToken);

            return new DeactivateUserResponse(DeactivateUserStatus.Ok);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    private DeactivateUserResponse ValidateDectivateUserAttempt(DeactivateUserRequest request)
    {
        if (!_identityService.VerifyAccessForAdmin(request.Id))
        {
            return new DeactivateUserResponse(DeactivateUserStatus.AccessDenied);
        }

        if (request.Id < 1)
        {
            return new DeactivateUserResponse(DeactivateUserStatus.InvalidRequest);
        }

        return new DeactivateUserResponse(DeactivateUserStatus.Ok);
    }
}