using Microsoft.EntityFrameworkCore;
using Psinder.DB.Common.Entities;

namespace Psinder.DB.Auth.Entities;

public class RolesDictionary : Dictionary, IEntity
{
    public static void SetEntity(ModelBuilder builder)
    {
        ConfigureEntity(builder);
        SeedEntity(builder);
    }

    public static void ConfigureEntity(ModelBuilder builder)
    {
        builder.Entity<RolesDictionary>(b =>
        {
            b.ToTable("dictionary_roles");
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
        foreach (Role status in Enum.GetValues(typeof(Role)))
        {
            builder.Entity<RolesDictionary>().HasData
            (
                new RolesDictionary { Id = (byte)status, Value = status.ToString() }
            );
        }
    }
}

public enum Role
{
    Admin = 1,
    User = 2,
    Worker = 3
}
