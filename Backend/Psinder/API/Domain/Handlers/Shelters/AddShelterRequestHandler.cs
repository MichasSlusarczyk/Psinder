using MediatR;
using Psinder.API.Domain.Models.Users;
using Psinder.DB.Domain.Repositories.Shelters;

namespace Psinder.API.Domain.Handlers;

public class AddShelterRequestHandler : IRequestHandler<AddShelterMediatr, AddShelterResponse>
{
    private readonly IShelterRepository _shelterRepository;
    private readonly ILogger<AddShelterRequestHandler> _logger;

    public AddShelterRequestHandler(
        IShelterRepository shelterRepository,
        ILogger<AddShelterRequestHandler> logger)
    {
        _shelterRepository = shelterRepository;
        _logger = logger;
    }

    public async Task<AddShelterResponse> Handle(AddShelterMediatr requestMediatr, CancellationToken cancellationToken)
    {
        try
        {
            var request = requestMediatr.Request;
            var domainModel = AddShelterRequest.ToDomain(request);
            var shelterData = await _shelterRepository.AddShelter(domainModel, cancellationToken);

            return AddShelterResponse.Create(AddShelterStatus.Ok, shelterData.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}