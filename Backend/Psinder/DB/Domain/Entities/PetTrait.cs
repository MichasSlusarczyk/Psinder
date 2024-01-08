using Microsoft.EntityFrameworkCore;

namespace Psinder.DB.Domain.Entities;

public class PetTrait
{
    public long PetId { get; set; }

    public byte TraitId { get; set; }

    public virtual Pet Pet { get; set; }

    public virtual PetTraitsDictionary Trait { get; set; }

    public static void SetEntity(ModelBuilder builder)
    {
        ConfigureEntity(builder);
        SeedEntity(builder);
    }

    public static void ConfigureEntity(ModelBuilder builder)
    {
        builder.Entity<PetTrait>(b =>
        {
            b.ToTable("pets_traits");
            b.Property(x => x.PetId)
                .HasColumnName("pet_id")
                .HasColumnType("bigint")
                .IsRequired()
                .HasComment("Pet id.");
            b.Property(x => x.TraitId)
                .HasColumnName("trait_id")
                .HasColumnType("tinyint")
                .IsRequired()
                .HasComment("Trait id.");
            b.HasKey(x => new { x.PetId, x.TraitId });
            b.HasOne(x => x.Pet)
                .WithMany(x => x.PetTraits)
                .HasForeignKey(x => x.PetId);
            b.HasOne(x => x.Trait)
                .WithMany()
                .HasForeignKey(x => x.TraitId);
        });
    }

    public static void SeedEntity(ModelBuilder builder)
    {
        builder.Entity<PetTrait>().HasData
        (
            new PetTrait()
            {
                PetId = 1,
                TraitId = 1
            },
            new PetTrait()
            {
                PetId = 1,
                TraitId = 2
            },
            new PetTrait()
            {
                PetId = 1,
                TraitId = 8
            },
            new PetTrait()
            {
                PetId = 1,
                TraitId = 11
            },
            new PetTrait()
            {
                PetId = 1,
                TraitId = 5
            },
            new PetTrait()
            {
                PetId = 2,
                TraitId = 9
            },
            new PetTrait()
            {
                PetId = 2,
                TraitId = 7
            },
            new PetTrait()
            {
                PetId = 2,
                TraitId = 4
            },
            new PetTrait()
            {
                PetId = 2,
                TraitId = 3
            },
            new PetTrait()
            {
                PetId = 3,
                TraitId = 3
            },
            new PetTrait()
            {
                PetId = 3,
                TraitId = 4
            },
            new PetTrait()
            {
                PetId = 3,
                TraitId = 6
            },
            new PetTrait()
            {
                PetId = 3,
                TraitId = 7
            },
            new PetTrait()
            {
                PetId = 4,
                TraitId = 4
            },
            new PetTrait()
            {
                PetId = 4,
                TraitId = 2
            },
            new PetTrait()
            {
                PetId = 4,
                TraitId = 9
            },
            new PetTrait()
            {
                PetId = 5,
                TraitId = 1
            },
            new PetTrait()
            {
                PetId = 5,
                TraitId = 2
            },
            new PetTrait()
            {
                PetId = 5,
                TraitId = 10
            },
            new PetTrait()
            {
                PetId = 5,
                TraitId = 9
            },
            new PetTrait()
            {
                PetId = 5,
                TraitId = 3
            },
            new PetTrait()
            {
                PetId = 6,
                TraitId = 3
            },
            new PetTrait()
            {
                PetId = 6,
                TraitId = 9
            },
            new PetTrait()
            {
                PetId = 6,
                TraitId = 11
            }
        );
    }
}
