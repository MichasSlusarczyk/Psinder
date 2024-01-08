using Psinder.DB.Domain.Entities;

namespace Psinder.API.Domain.Models.Pets;

public class AddPetRequest
{
    public AddPetRequestContent Content { get; set; }

    public List<IFormFile> Attachments { get; set; }

    public class AddPetRequestContent
    {
        public long ShelterId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Breed { get; set; }

        public int YearOfBirth { get; set; }

        public string Number { get; set; }

        public PetGenders Gender { get; set; }

        public PetSizes Size { get; set; }

        public PhysicalActivities PhysicalActivity { get; set; }

        public AttitudesTowardsChildren AttitudeTowardsChildren { get; set; }

        public AttitudesTowardsOtherDogs AttitudeTowardsOtherDogs { get; set; }

        public List<PetTraits> PetTraits { get; set; }
    }

    public static Pet ToDomain(AddPetRequest request)
    {
        var result = new Pet()
        {
            AttitudeTowardsChildren = request.Content.AttitudeTowardsChildren,
            Breed = request.Content.Breed,
            AttitudeTowardsOtherDogs = request.Content.AttitudeTowardsOtherDogs,
            Description = request.Content.Description,
            Gender = request.Content.Gender,
            Name = request.Content.Name,
            Number = request.Content.Number,
            Size = request.Content.Size,
            YearOfBirth = request.Content.YearOfBirth,
            PhysicalActivity = request.Content.PhysicalActivity,
            ShelterId = request.Content.ShelterId,
            PetImages = new List<PetImage>()
        };

        result.PetTraits = new List<PetTrait>();
        request.Content.PetTraits.ForEach(trait =>
        {
            var petTrait = new PetTrait()
            {
                TraitId = (byte)trait
            };

            result.PetTraits.Add(petTrait);
        });

        return result;
    }
}