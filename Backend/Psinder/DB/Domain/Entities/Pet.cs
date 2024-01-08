using Microsoft.EntityFrameworkCore;

namespace Psinder.DB.Domain.Entities;

public class Pet
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

    public virtual Shelter Shelter { get; set; }

    public virtual List<PetTrait> PetTraits { get; set; }

    public virtual List<Appointment>? Appointments { get; set; }

    public virtual List<PetImage>? PetImages { get; set; }

    public static void SetEntity(ModelBuilder builder)
    {
        ConfigureEntity(builder);
        SeedEntity(builder);
    }

    public static void ConfigureEntity(ModelBuilder builder)
    {
        builder.Entity<Pet>(b =>
        {
            b.ToTable("pets");
            b.HasKey(x => x.Id);
            b.Property(x => x.ShelterId)
                .HasColumnName("shelter_id")
                .HasColumnType("bigint")
                .IsRequired()
                .HasComment("Shelter id.");
            b.Property(x => x.Name)
                .HasColumnName("name")
                .HasColumnType("varchar")
                .IsRequired()
                .HasComment("Name.")
                .HasMaxLength(64);
            b.Property(x => x.Description)
                .HasColumnName("description")
                .HasColumnType("varchar")
                .IsRequired()
                .HasComment("Description.")
                .HasMaxLength(1024);
            b.Property(x => x.Breed)
                .HasColumnName("breed")
                .HasColumnType("varchar")
                .IsRequired()
                .HasComment("Breed.")
                .HasMaxLength(128);
            b.Property(x => x.YearOfBirth)
                .HasColumnName("year_of_birth")
                .HasColumnType("integer")
                .IsRequired()
                .HasComment("Year of birth.");
            b.Property(x => x.Number)
                .HasColumnName("number")
                .HasColumnType("varchar")
                .IsRequired()
                .HasComment("Number of a pet.")
                .HasMaxLength(16);
            b.Property(x => x.Gender)
                .HasColumnName("gender")
                .HasColumnType("smallint")
                .IsRequired()
                .HasConversion(x => (int)x, c => (PetGenders)c)
                .HasComment("Pet gender.");
            b.Property(x => x.Size)
                .HasColumnName("size")
                .HasColumnType("smallint")
                .IsRequired()
                .HasConversion(x => (int)x, c => (PetSizes)c)
                .HasComment("Pet size.");
            b.Property(x => x.PhysicalActivity)
                .HasColumnName("physical_activity")
                .HasColumnType("smallint")
                .IsRequired()
                .HasConversion(x => (int)x, c => (PhysicalActivities)c)
                .HasComment("Physical activity.");
            b.Property(x => x.AttitudeTowardsChildren)
                .HasColumnName("attitude_towards_children")
                .HasColumnType("smallint")
                .IsRequired()
                .HasConversion(x => (int)x, c => (AttitudesTowardsChildren)c)
                .HasComment("Attitude towards children.");
            b.Property(x => x.AttitudeTowardsOtherDogs)
                .HasColumnName("attitude_towards_other_dogs")
                .HasColumnType("smallint")
                .IsRequired()
                .HasConversion(x => (int)x, c => (AttitudesTowardsOtherDogs)c)
                .HasComment("Attitude towards other dogs.");
        });
    }

    public static void SeedEntity(ModelBuilder builder)
    {
        builder.Entity<Pet>().HasData
        (
            new Pet()
            {
                Id = 1,
                AttitudeTowardsChildren = AttitudesTowardsChildren.GoodWithChildren,
                AttitudeTowardsOtherDogs = AttitudesTowardsOtherDogs.NoDogs,
                Breed = "Husky",
                Gender = PetGenders.Male,
                Description = "Nice doggo",
                Name = "Ben",
                Number = "21345",
                PhysicalActivity = PhysicalActivities.Small,
                ShelterId = 1,
                Size = PetSizes.Large,
                YearOfBirth = 2014
            },
            new Pet()
            {
                Id = 2,
                AttitudeTowardsChildren = AttitudesTowardsChildren.NotGoodWithChildren,
                AttitudeTowardsOtherDogs = AttitudesTowardsOtherDogs.GoodWithOtherPets,
                Breed = "Shibe",
                Gender = PetGenders.Female,
                Description = "Nice doggo",
                Name = "Harry",
                Number = "65734",
                PhysicalActivity = PhysicalActivities.Medium,
                ShelterId = 2,
                Size = PetSizes.Small,
                YearOfBirth = 2019
            },
            new Pet()
            {
                Id = 3,
                AttitudeTowardsChildren = AttitudesTowardsChildren.NotGoodWithChildren,
                AttitudeTowardsOtherDogs = AttitudesTowardsOtherDogs.GoodWithOtherDogs,
                Breed = "Malamute",
                Gender = PetGenders.Male,
                Description = "Nice doggo",
                Name = "Mike",
                Number = "54632",
                PhysicalActivity = PhysicalActivities.Small,
                ShelterId = 2,
                Size = PetSizes.Large,
                YearOfBirth = 2013
            },
            new Pet()
            {
                Id = 4,
                AttitudeTowardsChildren = AttitudesTowardsChildren.NotGoodWithChildren,
                AttitudeTowardsOtherDogs = AttitudesTowardsOtherDogs.GoodWithOtherPets,
                Breed = "Terier",
                Gender = PetGenders.Female,
                Description = "Nice doggo",
                Name = "Bill",
                Number = "76834",
                PhysicalActivity = PhysicalActivities.Large,
                ShelterId = 1,
                Size = PetSizes.Small,
                YearOfBirth = 2016
            },
            new Pet()
            {
                Id = 5,
                AttitudeTowardsChildren = AttitudesTowardsChildren.NotGoodWithChildren,
                AttitudeTowardsOtherDogs = AttitudesTowardsOtherDogs.GoodWithOtherDogs,
                Breed = "Bulldog",
                Gender = PetGenders.Female,
                Description = "Nice doggo",
                Name = "Elis",
                Number = "45321",
                PhysicalActivity = PhysicalActivities.Small,
                ShelterId = 2,
                Size = PetSizes.Large,
                YearOfBirth = 2020
            },
            new Pet()
            {
                Id = 6,
                AttitudeTowardsChildren = AttitudesTowardsChildren.NotGoodWithChildren,
                AttitudeTowardsOtherDogs = AttitudesTowardsOtherDogs.GoodWithOtherPets,
                Breed = "Shepard",
                Gender = PetGenders.Female,
                Description = "Nice doggo",
                Name = "Ana",
                Number = "76834",
                PhysicalActivity = PhysicalActivities.Large,
                ShelterId = 1,
                Size = PetSizes.Small,
                YearOfBirth = 2016
            }
        );
    }
}