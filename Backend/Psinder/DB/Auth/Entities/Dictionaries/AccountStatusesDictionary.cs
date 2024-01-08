using Microsoft.EntityFrameworkCore;
using Psinder.DB.Common.Entities;

namespace Psinder.DB.Auth.Entities;

public class AccountStatusesDictionary : Dictionary, IEntity
{
    public static void SetEntity(ModelBuilder builder)
    {
        ConfigureEntity(builder);
        SeedEntity(builder);
    }

    public static void ConfigureEntity(ModelBuilder builder)
    {
        builder.Entity<AccountStatusesDictionary>(b =>
        {
            b.ToTable("dictionary_account_statuses");
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
        foreach (AccountStatuses status in Enum.GetValues(typeof(AccountStatuses)))
        {
            builder.Entity<AccountStatusesDictionary>().HasData
            (
                new AccountStatusesDictionary { Id = (byte)status, Value = status.ToString() }
            );
        }
    }
}

public enum AccountStatuses
{
    Unconfirmed = 1,
    Active = 2,
    Blocked = 3,
    Inactive = 4
}