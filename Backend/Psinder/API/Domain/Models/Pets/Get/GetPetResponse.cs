using Psinder.DB.Domain.Entities;

namespace Psinder.API.Domain.Models.Pets;

public class GetPetResponse
{
    public GetPetResponseContent Content { get; set; }

    public List<GetPetResponseAttachment> Attachments { get; set; }

    public class GetPetResponseContent
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

    public class GetPetResponseAttachment
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Extension { get; set; }

        public long ContentLength { get; set; }

        public string ContentType { get; set; }
    }

    public static GetPetResponse FromDomain(Pet pet)
    {
        var result = new GetPetResponse()
        {
            Content = new GetPetResponseContent()
            {
                AttitudeTowardsChildren = pet.AttitudeTowardsChildren,
                AttitudeTowardsOtherDogs = pet.AttitudeTowardsOtherDogs,
                Breed = pet.Breed,
                Description = pet.Description,
                Gender = pet.Gender,
                Name = pet.Name,
                Number = pet.Number,
                PetTraits = pet.PetTraits.Select(x => (PetTraits)x.TraitId).ToList(),
                PhysicalActivity = pet.PhysicalActivity,
                ShelterId = pet.ShelterId,
                Size = pet.Size,
                YearOfBirth = pet.YearOfBirth
            },
            Attachments = new List<GetPetResponseAttachment>()
        };

        pet.PetImages!.ForEach(x =>
        {
            var attachment = new GetPetResponseAttachment()
            {
                ContentLength = x.File.ContentLength,
                ContentType = x.File.ContentType,
                Extension = x.File.Extension,
                Name = x.File.Name,
                Id = x.File.Id
            };

            result.Attachments.Add(attachment);
        });

        return result;
    }
}
