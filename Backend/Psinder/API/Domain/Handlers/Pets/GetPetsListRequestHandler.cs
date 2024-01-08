using MediatR;
using Microsoft.IdentityModel.Tokens;
using Psinder.API.Common.Helpers;
using Psinder.Db.Domain.Models.Pets;
using Psinder.DB.Domain.Entities;
using Psinder.DB.Domain.Repositories.Pets;

namespace Psinder.API.Domain.Handlers.Pets;

public class GetPetsListRequestHandler : IRequestHandler<GetPetsListMediatr, GetPetsListResponse>
{
    private readonly IPetRepository _petRepository;
    private readonly ILogger<GetPetsListRequestHandler> _logger;

    public GetPetsListRequestHandler(
        IPetRepository petRepository,
        ILogger<GetPetsListRequestHandler> logger)
    {
        _petRepository = petRepository;
        _logger = logger;
    }

    public async Task<GetPetsListResponse> Handle(GetPetsListMediatr requestMediatr, CancellationToken cancellationToken)
    {
        try
        {
            var petsList = await _petRepository.GetPets(requestMediatr, cancellationToken);

            foreach(var pet in petsList.List)
            {
                pet.PetTraits = await _petRepository.GetPetTraitsByPetId(pet.Id, cancellationToken) 
                    ?? new List<PetTraits>();
            }

            if(requestMediatr.Sorting != null)
            {
                if (requestMediatr.Filters != null && !requestMediatr.Filters.PetTraits.IsNullOrEmpty())
                {
                    foreach (var pet in petsList.List)
                    {
                        foreach (var trait in pet.PetTraits)
                        {
                            if (requestMediatr.Filters.PetTraits!.Contains(trait))
                            {
                                pet.Score += 5;
                            }
                        }
                    }
                }

                petsList.List = ListHelper<GetPetsListRowResponse, PetsListSortColumns>.OrderBy(petsList.List.ToList(), requestMediatr.Sorting);
            }

            petsList.List = ListHelper<GetPetsListRowResponse, PetsListSortColumns>.Page(petsList.List.ToList(), requestMediatr.Paging!);

            foreach (var pet in petsList.List)
            {
                var petById = await _petRepository.GetPet(pet.Id, cancellationToken)
                    ?? throw new Exception($"Pet with ID: {pet.Id} not found.");
                pet.FillData(petById);
                pet.Attachments = await _petRepository.GetPetImagesByPetId(pet.Id, cancellationToken) 
                    ?? new List<GetPetsListRowResponse.GetPetsListRowResponseAttachment>();
            }

            return petsList;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}