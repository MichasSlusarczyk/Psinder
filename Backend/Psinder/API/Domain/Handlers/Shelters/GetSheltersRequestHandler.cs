using MediatR;
using Psinder.API.Domain.Models.Shelters;
using Psinder.DB.Domain.Repositories.Shelters;

namespace Psinder.API.Domain.Handlers;

public class GetSheltersRequestHandler : IRequestHandler<GetSheltersMediatr, GetSheltersResponse>
{
    private readonly IShelterRepository _shelterRepository;
    private readonly ILogger<GetSheltersRequestHandler> _logger;

    public GetSheltersRequestHandler(
        IShelterRepository shelterRepository,
        ILogger<GetSheltersRequestHandler> logger)
    {
        _shelterRepository = shelterRepository;
        _logger = logger;
    }

    public async Task<GetSheltersResponse> Handle(GetSheltersMediatr requestMediatr, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _shelterRepository.GetAllShelters(cancellationToken);

            return GetSheltersResponse.FromDomain(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}