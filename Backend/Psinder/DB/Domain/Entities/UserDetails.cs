using Microsoft.EntityFrameworkCore;
using Psinder.DB.Common.Entities;

namespace Psinder.DB.Domain.Entities;

public class UserDetails : IEntity
{
    public long Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public string? Street { get; set; }
    public string? StreetNumber { get; set; }
    public DateTime? BirthDate { get; set; }
    public UserGenders? Gender { get; set; }

    public virtual User User { get; set; }

    public static void SetEntity(ModelBuilder builder)
    {
        ConfigureEntity(builder);
        SeedEntity(builder);
    }

    public static void ConfigureEntity(ModelBuilder builder)
    {
        builder.Entity<UserDetails>(b =>
        {
            b.ToTable("users_details");
            b.HasKey(x => x.Id);
            b.Property(x => x.FirstName)
                .HasColumnName("first_name")
                .HasColumnType("varchar")
                .HasComment("First name.")
                .HasMaxLength(64);
            b.Property(x => x.LastName)
                .HasColumnName("last_name")
                .HasColumnType("varchar")
                .HasComment("Last name.")
                .HasMaxLength(64);
            b.Property(x => x.PhoneNumber)
                .HasColumnName("phone_number")
                .HasColumnType("varchar")
                .HasComment("Phone number.")
                .HasMaxLength(9);
            b.Property(x => x.City)
                .HasColumnName("city")
                .HasColumnType("varchar")
                .HasComment("City.")
                .HasMaxLength(30);
            b.Property(x => x.PostalCode)
                .HasColumnName("postal_code")
                .HasColumnType("varchar")
                .HasComment("Postal code.")
                .HasMaxLength(6);
            b.Property(x => x.Street)
                .HasColumnName("street")
                .HasColumnType("varchar")
                .HasComment("Street.")
                .HasMaxLength(20);
            b.Property(x => x.BirthDate)
                .HasColumnName("birth_date")
                .HasColumnType("datetime")
                .HasComment("Birth date.");
            b.Property(x => x.Gender)
                .HasColumnName("gender")
                .HasColumnType("smallint")
                .HasConversion(x => (int)x, c => (UserGenders)c)
                .HasComment("User gender.");
        });
    }

    public static void SeedEntity(ModelBuilder builder)
    {
        builder.Entity<UserDetails>().HasData
        (
            new UserDetails()
            {
                Id = 1,
                FirstName = "User1",
                LastName = "User1",
                PhoneNumber = "123456789",
                Gender = UserGenders.Man,
                BirthDate = DateTime.Now.AddYears(20),
                City = "City",
                PostalCode = "43-300",
                Street = "Street",
                StreetNumber = "12"
            },
            new UserDetails()
            {
                Id = 2,
                FirstName = "User2",
                LastName = "User2",
                PhoneNumber = "123456789",
                Gender = UserGenders.Man,
                BirthDate = DateTime.Now.AddYears(20),
                City = "City",
                PostalCode = "43-300",
                Street = "Street",
                StreetNumber = "12"
            },
            new UserDetails()
            {
                Id = 3,
                FirstName = "User3",
                LastName = "User3",
                PhoneNumber = "123456789",
                Gender = UserGenders.Man,
                BirthDate = DateTime.Now.AddYears(20),
                City = "City",
                PostalCode = "43-300",
                Street = "Street",
                StreetNumber = "12"
            }
        );
    }
}
