using Microsoft.EntityFrameworkCore;
using Psinder.DB.Auth.Entities;
using Psinder.DB.Common.Entities;

namespace Psinder.DB.Domain.Entities;

public class User : IEntity
{
    public long Id { get; set; }
    public long? UserDetailsId { get; set; }
    public bool SignedForNewsletter { get; set; }
    public DateTime CreationDate { get; set; }

    public virtual UserDetails? UserDetails { get; set; }
    public virtual LoginData LoginData { get; set; }

    public static void SetEntity(ModelBuilder builder)
    {
        ConfigureEntity(builder);
        SeedEntity(builder);
    }

    public static void ConfigureEntity(ModelBuilder builder)
    {
        builder.Entity<User>(b =>
        {
            b.ToTable("users");
            b.HasKey(x => x.Id);
            b.Property(x => x.SignedForNewsletter)
                .HasColumnName("signed_for_newsletter")
                .HasColumnType("bit")
                .IsRequired()
                .HasComment("Is signed for newsletter.")
                .HasMaxLength(64);
            b.Property(x => x.CreationDate)
                .HasColumnName("creation_date")
                .HasColumnType("datetime")
                .IsRequired()
                .HasComment("Date of account creation.")
                .HasDefaultValueSql("GETDATE()");
            b.Ignore(x => x.CreationDate);
            b.HasOne(x => x.UserDetails)
                .WithOne(x => x.User)
                .HasForeignKey<User>(x => x.UserDetailsId);
        });
    }

    public static void SeedEntity(ModelBuilder builder)
    {
        builder.Entity<User>().HasData
        (
            new User
            {
                Id = 1,
                UserDetailsId = 1,
                CreationDate = DateTime.Now,
                SignedForNewsletter = true
            },
            new User
            {
                Id = 2,
                UserDetailsId = 2,
                CreationDate = DateTime.Now,
                SignedForNewsletter = true
            },
            new User
            {
                Id = 3,
                UserDetailsId = 3,
                CreationDate = DateTime.Now,
                SignedForNewsletter = true
            },
            new User
            {
                Id = 4,
                UserDetailsId = null,
                CreationDate = DateTime.Now,
                SignedForNewsletter = true
            },
            new User
            {
                Id = 5,
                UserDetailsId = null,
                CreationDate = DateTime.Now,
                SignedForNewsletter = true
            }
        );
    }

    public static User ToDomain(string email)
    {
        var domainModel = new User()
        {
            SignedForNewsletter = true,
            LoginData = new LoginData()
            {
                Email = email,
                RoleId = (byte)Role.User
            }
        };

        return domainModel;
    }
}
