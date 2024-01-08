using MediatR;
using Psinder.API.Common;
using Psinder.API.Domain.Models.Users;
using Psinder.DB.Auth.Entities;
using Psinder.DB.Auth.Models;
using Psinder.DB.Auth.Repositories.Logins;

namespace Psinder.API.Domain.Handlers;

public class BlockUserRequestHandler : IRequestHandler<BlockUserMediatr, BlockUserResponse>
{
    private readonly IIdentityService _identityService;
    private readonly ILoginRepository _loginRepository;
    private readonly ILogger<BlockUserRequestHandler> _logger;

    public BlockUserRequestHandler(
        IIdentityService identityService,
        ILoginRepository loginRepository,
        ILogger<BlockUserRequestHandler> logger)
    {
        _identityService = identityService;
        _loginRepository = loginRepository;
        _logger = logger;
    }

    public async Task<BlockUserResponse> Handle(BlockUserMediatr requestMediatr, CancellationToken cancellationToken)
    {
        try
        {
            var request = requestMediatr.Request;

            var passwordData = await _loginRepository.GetPasswordData(request.Id, cancellationToken)
                ?? throw new Exception($"Password data for user ID: {request.Id} not found.");

            var validationResult = ValidateBlockUserAttempt(request, passwordData);

            if (validationResult.Status != BlockUserStatus.Ok)
            {
                return validationResult;
            }

            await _loginRepository.UpdateAccountStatus(passwordData!.Id, AccountStatuses.Blocked, cancellationToken);

            return new BlockUserResponse(BlockUserStatus.Ok);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    private BlockUserResponse ValidateBlockUserAttempt(BlockUserRequest request, PasswordDto passwordData)
    {
        if (passwordData.AccountStatus == AccountStatuses.Blocked)
        {
            return new BlockUserResponse(BlockUserStatus.AlreadyBlocked);
        }

        if (_identityService.CurrentUserRole != Role.Admin)
        {
            return new BlockUserResponse(BlockUserStatus.AccessDenied);
        }

        if (request.Id < 1)
        {
            return new BlockUserResponse(BlockUserStatus.InvalidRequest);
        }

        return new BlockUserResponse(BlockUserStatus.Ok);
    }
}