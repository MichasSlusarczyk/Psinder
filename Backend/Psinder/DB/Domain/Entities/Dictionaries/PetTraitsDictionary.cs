using Microsoft.EntityFrameworkCore;
using Psinder.DB.Common.Entities;

namespace Psinder.DB.Domain.Entities;

public class PetTraitsDictionary : Dictionary, IEntity
{
public static void SetEntity(ModelBuilder builder)
{
    ConfigureEntity(builder);
    SeedEntity(builder);
}

public static void ConfigureEntity(ModelBuilder builder)
{
    builder.Entity<PetTraitsDictionary>(b =>
    {
        b.ToTable("dictionary_pet_traits");
        b.HasKey(x => x.Id);
        b.Property(x => x.Value)
            .HasColumnName("value")
            .HasColumnType("varchar")
            .IsRequired()
            .HasComment("Dictionary value.")
            .HasMaxLength(64);
    });
}

public static void SeedEntity(ModelBuilder builder)
{
    foreach (PetTraits status in Enum.GetValues(typeof(PetTraits)))
    {
        builder.Entity<PetTraitsDictionary>().HasData
        (
            new PetTraitsDictionary { Id = (byte)status, Value = status.ToString() }
        );
    }
}
}

public enum PetTraits
{
    DoesntBark = 1,
    DefendsTheHouse,
    KnowsCommands,
    LikesToPlay,
    Shy,
    SuitableAsFirstDog,
    ShortHaired,
    LongHaired,
    DoesNotShedFur,
    Submissive,
    Dominant
}