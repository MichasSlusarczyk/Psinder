using Microsoft.EntityFrameworkCore;

namespace Psinder.DB.Common.Entities;

public class Configuration : IEntity
{
    public long? Id { get; set; }
    public long ConfigurationCategoryId { get; set; }
    public long ConfigurationItemId { get; set; }
    public string Value { get; set; }
    public string? Description { get; set; }
    public bool Enabled { get; set; }

    public static void SetEntity(ModelBuilder builder)
    {
        ConfigureEntity(builder);
        SeedEntity(builder);
    }

    public static void ConfigureEntity(ModelBuilder builder)
    {
        builder.Entity<Configuration>(b =>
        {
            b.ToTable("configurations");
            b.HasKey(x => x.Id);
            b.Property(x => x.ConfigurationCategoryId)
                .HasColumnName("configuration_category_id")
                .HasColumnType("bigint")
                .IsRequired()
                .HasComment("Configuration category id.");
            b.Property(x => x.ConfigurationItemId)
                .HasColumnName("configuration_item_id")
                .HasColumnType("bigint")
                .IsRequired()
                .HasComment("Configuration item id.");
            b.Property(x => x.Value)
                .HasColumnName("value")
                .HasColumnType("varchar")
                .IsRequired()
                .HasComment("Value of configuration.")
                .HasMaxLength(256);
            b.Property(x => x.Description)
                .HasColumnName("description")
                .HasColumnType("varchar")
                .HasComment("Description of configuration.")
                .HasMaxLength(256);
            b.Property(x => x.Enabled)
                .HasColumnName("enabled")
                .HasColumnType("bit")
                .IsRequired()
                .HasComment("Whether the given configuration is valid.");
        });
    }

    public static void SeedEntity(ModelBuilder builder)
    {
        builder.Entity<Configuration>().HasData
        (
            new Configuration 
            { 
                Id = 1, 
                ConfigurationCategoryId = (long)ConfigurationCategory.LoginPassword, 
                ConfigurationItemId = (long)ConfigurationItem.LoginPasswordMinLength,
                Description = "Login password minimal length.",
                Value = "8",
                Enabled = true
            },
            new Configuration
            {
                Id = 2,
                ConfigurationCategoryId = (long)ConfigurationCategory.LoginPassword,
                ConfigurationItemId = (long)ConfigurationItem.LowercaseLettersMinCount,
                Description = "Lowercase letters minimal count in login password.",
                Value = "1",
                Enabled = true
            },
            new Configuration
            {
                Id = 3,
                ConfigurationCategoryId = (long)ConfigurationCategory.LoginPassword,
                ConfigurationItemId = (long)ConfigurationItem.UppercaseLettersMinCount,
                Description = "Uppercase letters minimal count in login password.",
                Value = "1",
                Enabled = true
            },
            new Configuration
            {
                Id = 4,
                ConfigurationCategoryId = (long)ConfigurationCategory.LoginPassword,
                ConfigurationItemId = (long)ConfigurationItem.DigitsMinCount,
                Description = "Digits minimal count in login password.",
                Value = "1",
                Enabled = true
            },
            new Configuration
            {
                Id = 5,
                ConfigurationCategoryId = (long)ConfigurationCategory.LoginPassword,
                ConfigurationItemId = (long)ConfigurationItem.SpecialCharactersMinCount,
                Description = "Special characters minimal count in login password.",
                Value = "1",
                Enabled = true
            },
            new Configuration
            {
                Id = 6,
                ConfigurationCategoryId = (long)ConfigurationCategory.Login,
                ConfigurationItemId = (long)ConfigurationItem.BadLoginAtteptsCount,
                Description = "Bad login attempts possible count.",
                Value = "5",
                Enabled = true
            },
            new Configuration
            {
                Id = 7,
                ConfigurationCategoryId = (long)ConfigurationCategory.Token,
                ConfigurationItemId = (long)ConfigurationItem.LoginTokenExpireTimeInMinutes,
                Description = "Login token expire time in minutes.",
                Value = "60",
                Enabled = true
            },
            new Configuration
            {
                Id = 8,
                ConfigurationCategoryId = (long)ConfigurationCategory.Token,
                ConfigurationItemId = (long)ConfigurationItem.RefreshTokenExpireTimeInMinutes,
                Description = "Refresh token expire time in minutes.",
                Value = "10080",
                Enabled = true
            },
            new Configuration
            {
                Id = 9,
                ConfigurationCategoryId = (long)ConfigurationCategory.Token,
                ConfigurationItemId = (long)ConfigurationItem.VerificationTokenExpireTimeInMinutes,
                Description = "Verification token expire time in minutes.",
                Value = "1440",
                Enabled = true
            },
            new Configuration
            {
                Id = 10,
                ConfigurationCategoryId = (long)ConfigurationCategory.Token,
                ConfigurationItemId = (long)ConfigurationItem.ResetPasswordTokenExpireTimeInMinutes,
                Description = "Reset password token expire time in minutes.",
                Value = "30",
                Enabled = true
            },
            new Configuration
            {
                Id = 11,
                ConfigurationCategoryId = (long)ConfigurationCategory.AccessKeyData,
                ConfigurationItemId = (long)ConfigurationItem.AccessKeyLength,
                Description = "Length of access key to user passwords.",
                Value = "21",
                Enabled = true
            },
            new Configuration
            {
                Id = 12,
                ConfigurationCategoryId = (long)ConfigurationCategory.AccessKeyData,
                ConfigurationItemId = (long)ConfigurationItem.AccessSaltLength,
                Description = "Length of access salt to user passwords.",
                Value = "16",
                Enabled = true
            },
            new Configuration
            {
                Id = 13,
                ConfigurationCategoryId = (long)ConfigurationCategory.AccessKeyData,
                ConfigurationItemId = (long)ConfigurationItem.AccessIVLength,
                Description = "Length of access IV to user passwords.",
                Value = "16",
                Enabled = true
            }
        );
    }
}

public enum ConfigurationCategory
{
    LoginPassword = 1,
    Login = 2,
    Token = 3,
    AccessKeyData = 4
}

public enum ConfigurationItem
{
    LoginPasswordMinLength = 1,
    LowercaseLettersMinCount = 2,
    UppercaseLettersMinCount = 3,
    DigitsMinCount = 4,
    SpecialCharactersMinCount = 5,
    BadLoginAtteptsCount = 6,
    LoginTokenExpireTimeInMinutes = 7,
    RefreshTokenExpireTimeInMinutes = 8,
    VerificationTokenExpireTimeInMinutes = 9,
    ResetPasswordTokenExpireTimeInMinutes = 10,
    AccessKeyLength = 11,
    AccessIVLength = 12,
    AccessSaltLength = 13
}
