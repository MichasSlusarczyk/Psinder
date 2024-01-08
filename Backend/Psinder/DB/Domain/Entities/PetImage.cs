using Microsoft.EntityFrameworkCore;

namespace Psinder.DB.Domain.Entities;

public class PetImage
{
    public long PetId { get; set; }

    public long FileId { get; set; }

    public virtual Pet Pet { get; set; }

    public virtual Psinder.DB.Common.Entities.File File { get; set; }

    public static void SetEntity(ModelBuilder builder)
    {
        ConfigureEntity(builder);
        SeedEntity(builder);
    }

    public static void ConfigureEntity(ModelBuilder builder)
    {
        builder.Entity<PetImage>(b =>
        {
            b.ToTable("pets_images");
            b.Property(x => x.PetId)
                .HasColumnName("pet_id")
                .HasColumnType("bigint")
                .IsRequired()
                .HasComment("Pet id.");
            b.Property(x => x.FileId)
                .HasColumnName("file_id")
                .HasColumnType("bigint")
                .IsRequired()
                .HasComment("File id.");
            b.HasKey(x => new { x.PetId, x.FileId });
            b.HasOne(x => x.Pet)
                .WithMany(x => x.PetImages)
                .HasForeignKey(x => x.PetId);
            b.HasOne(x => x.File)
                .WithMany()
                .HasForeignKey(x => x.FileId);
        });
    }

    public static void SeedEntity(ModelBuilder builder)
    {
        builder.Entity<PetImage>().HasData
        (
            new PetImage()
            {
                PetId = 1,
                FileId = 1
            },
            new PetImage()
            {
                PetId = 2,
                FileId = 2
            },
            new PetImage()
            {
                PetId = 3,
                FileId = 3
            },
            new PetImage()
            {
                PetId = 4,
                FileId = 4
            },
            new PetImage()
            {
                PetId = 5,
                FileId = 5
            },
            new PetImage()
            {
                PetId = 6,
                FileId = 6
            },
            new PetImage()
            {
                PetId = 1,
                FileId = 7
            }
        );
    }
}
