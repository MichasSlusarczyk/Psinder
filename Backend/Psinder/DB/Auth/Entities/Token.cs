using Microsoft.EntityFrameworkCore;
using Psinder.DB.Common.Entities;

namespace Psinder.DB.Auth.Entities;

public class Token : IEntity
{
    public long Id { get; set; }
    public string Value { get; set; }
    public DateTime ExpirationDate { get; set; }
    public DateTime CreationDate { get; set; }

    public static void SetEntity(ModelBuilder builder)
    {
        ConfigureEntity(builder);
    }

    public static void ConfigureEntity(ModelBuilder builder)
    {
        builder.Entity<Token>(b =>
        {
            b.ToTable("tokens");
            b.HasKey(x => x.Id);
            b.Property(x => x.Value)
                .HasColumnName("value")
                .HasColumnType("varchar")
                .IsRequired()
                .HasComment("Value of the token.")
                .HasMaxLength(256);
            b.Property(x => x.ExpirationDate)
                .HasColumnName("expiration_date")
                .HasColumnType("datetime")
                .HasComment("Date of token expiration.")
                .IsRequired();
            b.Property(x => x.CreationDate)
                .HasColumnName("creation_date")
                .HasColumnType("datetime")
                .IsRequired()
                .HasComment("Date of token creation.");
        });
    }
}
