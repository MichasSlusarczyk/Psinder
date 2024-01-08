using Psinder.DB.Domain.Entities;

namespace Psinder.API.Domain.Models.Pets;

public class UpdatePetRequest
{
    public UpdatePetRequestContent Content { get; set; }

    public List<IFormFile>? AttachmentsToAdd { get; set; }
    public List<long>? AttachmentsToDelete { get; set; }

    public class UpdatePetRequestContent
    {
        public long Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Breed { get; set; }

        public int? YearOfBirth { get; set; }

        public string? Number { get; set; }

        public PetGenders? Gender { get; set; }

        public PetSizes? Size { get; set; }

        public PhysicalActivities? PhysicalActivity { get; set; }

        public AttitudesTowardsChildren? AttitudeTowardsChildren { get; set; }

        public AttitudesTowardsOtherDogs? AttitudeTowardsOtherDogs { get; set; }

        public List<PetTraits>? PetTraitsToAdd { get; set; }

        public List<PetTraits>? PetTraitsToDelete { get; set; }
    }
}