using Microsoft.EntityFrameworkCore;

namespace Psinder.DB.Domain.Entities;

public class Worker
{
    public long UserId { get; set; }

    public long ShelterId { get; set; }

    public virtual User User { get; set; }

    public virtual Shelter Shelter { get; set; }

    public static void SetEntity(ModelBuilder builder)
    {
        ConfigureEntity(builder);
        SeedEntity(builder);
    }

    public static void ConfigureEntity(ModelBuilder builder)
    {
        builder.Entity<Worker>(b =>
        {
            b.ToTable("workers");
            b.Property(x => x.UserId)
                .HasColumnName("user_id")
                .HasColumnType("bigint")
                .IsRequired()
                .HasComment("User id.");
            b.Property(x => x.ShelterId)
                .HasColumnName("shelter_id")
                .HasColumnType("bigint")
                .IsRequired()
                .HasComment("Shelter id.");
            b.HasKey(x => new { x.ShelterId, x.UserId });
            b.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);
            b.HasOne(x => x.Shelter)
                .WithMany(x => x.Workers)
                .HasForeignKey(x => x.ShelterId);
        });
    }

    public static void SeedEntity(ModelBuilder builder)
    {
        builder.Entity<Worker>().HasData
        (
            new Worker()
            {
                UserId = 3,
                ShelterId = 1
            },
            new Worker()
            {
                UserId = 5,
                ShelterId = 2
            }
        );
    }
}
