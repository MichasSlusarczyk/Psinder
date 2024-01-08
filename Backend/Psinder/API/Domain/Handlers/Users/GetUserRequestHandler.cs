using MediatR;
using Psinder.API.Common;
using Psinder.API.Common.Exceptions;
using Psinder.API.Domain.Models.Users;
using Psinder.DB.Domain.Repositories.Users;

namespace Psinder.API.Domain.Handlers;

public class GetUserRequestHandler : IRequestHandler<GetUserMediatr, GetUserResponse>
{
    private readonly IIdentityService _identityService;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<GetUserRequestHandler> _logger;

    public GetUserRequestHandler(
        IIdentityService identityService,
        IUserRepository userRepository,
        ILogger<GetUserRequestHandler> logger)
    {
        _identityService = identityService;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<GetUserResponse> Handle(GetUserMediatr requestMediatr, CancellationToken cancellationToken)
    {
        try
        {
            var request = requestMediatr.Request;

            if (!_identityService.VerifyAccessForWorkerAndAdmin(request.Id))
            {
                throw new AccessDeniedException($"For a logged in user with role: {_identityService.CurrentUserRole} and ID: {_identityService.CurrentUserId} no access to data of user with ID: {request.Id}.");
            }

            var userData = await _userRepository.GetUserById(request.Id, cancellationToken)
                ?? throw new Exception($"No user data found for ID: {request.Id}.");

            var shelterId = await _userRepository.GetWorkerShelterId(userData.Id, cancellationToken);

            return GetUserResponse.FromDomain(userData, shelterId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}