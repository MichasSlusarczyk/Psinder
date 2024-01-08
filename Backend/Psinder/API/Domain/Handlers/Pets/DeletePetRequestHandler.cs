using MediatR;
using Psinder.API.Common;
using Psinder.API.Domain.Models.Pets;
using Psinder.DB.Domain.Repositories.Pets;
using Psinder.DB.Domain.Repositories.Shelters;

namespace Psinder.API.Domain.Handlers.Pets;

public class DeletePetRequestHandler : IRequestHandler<DeletePetMediatr, Unit>
{
    private readonly IPetRepository _petRepository;
    private readonly IShelterRepository _shelterRepository;
    private readonly IIdentityService _identityService;
    private readonly ILogger<DeletePetRequestHandler> _logger;

    public DeletePetRequestHandler(
        IPetRepository petRepository,
        IIdentityService identityService,
        IShelterRepository shelterRepository,
        ILogger<DeletePetRequestHandler> logger)
    {
        _petRepository = petRepository;
        _identityService = identityService;
        _shelterRepository = shelterRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeletePetMediatr requestMediatr, CancellationToken cancellationToken)
    {
        try
        {
            var request = requestMediatr.Request;

            var loggedUserId = _identityService.CurrentUserId
                ?? throw new Exception("No access token with logged user ID.");

            var pet = await _petRepository.GetPet(request.PetId, cancellationToken)
                ?? throw new Exception($"Pet with ID: {request.PetId} not found.");

            if (await _shelterRepository.CheckWorkerAccessToShelter(loggedUserId, pet.ShelterId, cancellationToken))
            {
                await _petRepository.DeletePet(pet, cancellationToken);
            }

            return new Unit();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}