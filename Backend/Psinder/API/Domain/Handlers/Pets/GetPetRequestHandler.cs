using MediatR;
using Psinder.API.Domain.Models.Pets;
using Psinder.DB.Domain.Repositories.Pets;

namespace Psinder.API.Domain.Handlers.Pets;

public class GetPetRequestHandler : IRequestHandler<GetPetMediatr, GetPetResponse>
{
    private readonly IPetRepository _petRepository;
    private readonly ILogger<GetPetRequestHandler> _logger;

    public GetPetRequestHandler(
        IPetRepository petRepository,
        ILogger<GetPetRequestHandler> logger)
    {
        _petRepository = petRepository;
        _logger = logger;
    }

    public async Task<GetPetResponse> Handle(GetPetMediatr requestMediatr, CancellationToken cancellationToken)
    {
        try
        {
            var request = requestMediatr.Request;

            var pet = await _petRepository.GetPet(request.PetId, cancellationToken)
                ?? throw new Exception($"Pet with ID: {request.PetId} not found.");

            return GetPetResponse.FromDomain(pet);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}