using MediatR;
using Psinder.API.Domain.Models.Users;
using Psinder.DB.Domain.Repositories.Shelters;

namespace Psinder.API.Domain.Handlers.Shelters;

public class GetShelterRequestHandler : IRequestHandler<GetShelterMediatr, GetShelterResponse>
{
    private readonly IShelterRepository _shelterRepository;
    private readonly ILogger<GetShelterRequestHandler> _logger;

    public GetShelterRequestHandler(
        IShelterRepository shelterRepository,
        ILogger<GetShelterRequestHandler> logger)
    {
        _shelterRepository = shelterRepository;
        _logger = logger;
    }

    public async Task<GetShelterResponse> Handle(GetShelterMediatr requestMediatr, CancellationToken cancellationToken)
    {
        try
        {
            var request = requestMediatr.Request;

            var shelterData = await _shelterRepository.GetShelterById(request.Id, cancellationToken)
                ?? throw new Exception($"No shelter data found for ID: {request.Id}.");

            return GetShelterResponse.FromDomain(shelterData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}