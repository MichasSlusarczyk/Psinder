using Psinder.Db.Domain.Models.Pets;
using Psinder.DB.Domain.Entities;

namespace Psinder.DB.Domain.Repositories.Pets;

public interface IPetRepository
{
    Task<long> AddPet(Pet pet, CancellationToken cancellationToken);

    Task<Pet> GetPet(long petId, CancellationToken cancellationToken);

    Task<GetPetsListResponse> GetPets(GetPetsListMediatr request, CancellationToken cancellationToken);

    Task UpdatePet(Pet pet, CancellationToken cancellationToken);

    Task DeletePet(Pet pet, CancellationToken cancellationToken);

    Task<List<PetTraits>?> GetPetTraitsByPetId(long petId, CancellationToken cancellationToken);

    Task<List<GetPetsListRowResponse.GetPetsListRowResponseAttachment>?> GetPetImagesByPetId(long petId, CancellationToken cancellationToken);

    Task AddPetImage(PetImage petImage, CancellationToken cancellationToken);

    Task DeletePetImage(PetImage petImage, CancellationToken cancellationToken);

    Task AddPetTrait(PetTrait petTrait, CancellationToken cancellationToken);

    Task DeletePetTrait(PetTrait petTrait, CancellationToken cancellationToken);
}