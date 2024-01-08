using MediatR;
using Psinder.API.Common;
using Psinder.API.Domain.Models.Shelters;
using Psinder.DB.Domain.Repositories.Shelters;

namespace Psinder.API.Domain.Handlers;

public class GetSheltersForWorkerRequestHandler : IRequestHandler<GetSheltersForWorkerMediatr, GetSheltersResponse>
{
    private readonly IIdentityService _identityService;
    private readonly IShelterRepository _shelterRepository;
    private readonly ILogger<GetSheltersForWorkerRequestHandler> _logger;

    public GetSheltersForWorkerRequestHandler(
        IIdentityService identityService,
        IShelterRepository shelterRepository,
        ILogger<GetSheltersForWorkerRequestHandler> logger)
    {
        _identityService = identityService;
        _shelterRepository = shelterRepository;
        _logger = logger;
    }

    public async Task<GetSheltersResponse> Handle(GetSheltersForWorkerMediatr requestMediatr, CancellationToken cancellationToken)
    {
        try
        {
            var loggedUserId = _identityService.CurrentUserId
                ?? throw new Exception("No access token with logged user ID.");

            var result = await _shelterRepository.GetAllSheltersForWorker(loggedUserId, cancellationToken);

            return GetSheltersResponse.FromDomain(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}