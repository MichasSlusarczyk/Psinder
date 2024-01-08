using Psinder.DB.Common.Searching;
using Psinder.DB.Domain.Entities;

namespace Psinder.Db.Domain.Models.Pets;

public class GetPetsListResponse : Page<GetPetsListRowResponse>
{
}

public class GetPetsListRowResponse
{
    public long Id { get; set; }

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

    public int Score { get; set; }

    public List<GetPetsListRowResponseAttachment> Attachments { get; set; }

    public class GetPetsListRowResponseAttachment
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Extension { get; set; }

        public long ContentLength { get; set; }

        public string ContentType { get; set; }
    }

    public void FillData(Pet pet)
    {
        ShelterId = pet.ShelterId;
        Name = pet.Name;
        Description = pet.Description;
        Breed = pet.Breed;
        YearOfBirth = pet.YearOfBirth;
        Number = pet.Number;
        Gender = pet.Gender;
        Size = pet.Size;
        PhysicalActivity = pet.PhysicalActivity;
        AttitudeTowardsChildren = pet.AttitudeTowardsChildren;
        AttitudeTowardsOtherDogs = pet.AttitudeTowardsOtherDogs;
    }
}