using Microsoft.EntityFrameworkCore;

namespace Psinder.DB.Domain.Entities;

public class Shelter
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string City { get; set; }

    public string Address { get; set; }

    public string Description { get; set; }

    public string PhoneNumber { get; set; }

    public string Email { get; set; }

    public virtual List<Worker>? Workers { get; set; }

    public virtual List<Pet>? Pets { get; set; }

    public static void SetEntity(ModelBuilder builder)
    {
        ConfigureEntity(builder);
        SeedEntity(builder);
    }

    public static void ConfigureEntity(ModelBuilder builder)
    {
        builder.Entity<Shelter>(b =>
        {
            b.ToTable("shelters");
            b.HasKey(x => x.Id);
            b.Property(x => x.Name)
                .HasColumnName("name")
                .HasColumnType("varchar")
                .IsRequired()
                .HasComment("Name.")
                .HasMaxLength(64);
            b.Property(x => x.City)
                .HasColumnName("city")
                .HasColumnType("varchar")
                .IsRequired()
                .HasComment("City.")
                .HasMaxLength(32);
            b.Property(x => x.Address)
                .HasColumnName("address")
                .HasColumnType("varchar")
                .IsRequired()
                .HasComment("Address.")
                .HasMaxLength(128);
            b.Property(x => x.Description)
                .HasColumnName("description")
                .HasColumnType("varchar")
                .IsRequired()
                .HasComment("Description.")
                .HasMaxLength(1024);
            b.Property(x => x.PhoneNumber)
                .HasColumnName("phone_number")
                .HasColumnType("varchar")
                .IsRequired()
                .HasComment("Phone number.")
                .HasMaxLength(9);
            b.Property(x => x.Email)
                .HasColumnName("email")
                .HasColumnType("varchar")
                .IsRequired()
                .HasComment("Email.")
                .HasMaxLength(64);
            b.HasMany(x => x.Pets)
                .WithOne(x => x.Shelter)
                .HasForeignKey(x => x.ShelterId);
        });
    }

    public static void SeedEntity(ModelBuilder builder)
    {
        builder.Entity<Shelter>().HasData
        (
            new Shelter()
            {
                Id = 1,
                Address = "Akademicka 16",
                City = "Gliwice",
                Description = "Super shelter!",
                Email = "shelter1@gmail.com",
                Name = "Academic shelter",
                PhoneNumber = "123456789"
            },
            new Shelter()
            {
                Id = 2,
                Address = "Stawowa 5",
                City = "Katowice",
                Description = "Really nice shleter",
                Email = "shelter2@gmail.com",
                Name = "Chonky doggos shelter",
                PhoneNumber = "123456789"
            }
        );
    }
}
