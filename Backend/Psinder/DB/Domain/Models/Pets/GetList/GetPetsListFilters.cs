using Psinder.DB.Domain.Entities;

namespace Psinder.Db.Domain.Models.Pets;

public class GetPetsListFilters
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Breed { get; set; }

    public int? YearOfBirth { get; set; }

    public string? Number { get; set; }

    public long? ShelterId { get; set; }

    public PetGenders? Gender { get; set; }

    public PetSizes? Size { get; set; }

    public PhysicalActivities? PhysicalActivity { get; set; }

    public AttitudesTowardsChildren? AttitudeTowardsChildren { get; set; }

    public AttitudesTowardsOtherDogs? AttitudeTowardsOtherDogs { get; set; }

    public List<string>? Cities { get; set; }

    public List<PetTraits>? PetTraits { get; set; }
}
