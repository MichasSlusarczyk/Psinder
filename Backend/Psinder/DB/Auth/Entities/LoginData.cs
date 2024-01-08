using Microsoft.EntityFrameworkCore;
using Psinder.DB.Common.Entities;
using Psinder.DB.Common.Extensions;
using Psinder.DB.Domain.Entities;
using System.Text;

namespace Psinder.DB.Auth.Entities;

public class LoginData : IEntity
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string Email { get; set; }
    public byte[]? Salt { get; set; }
    public byte[]? PasswordHash { get; set; }
    public byte[]? AccessKey { get; set; }
    public byte[]? AccessIV { get; set; }
    public byte[]? AccessSalt { get; set; }
    public DateTime? PasswordLastChangeDate { get; set; }
    public byte LoginAttempts { get; set; }
    public byte RoleId { get; set; }
    public byte AccountStatusId { get; set; }
    public long? RegisterVerificationTokenId { get; set; }
    public long? ResetPasswordTokenId { get; set; }
    public long? RefreshTokenId { get; set; }

    public virtual User User { get; set; }
    public virtual RolesDictionary Role { get; set; }
    public virtual AccountStatusesDictionary AccountStatus { get; set; }
    public virtual Token? RegisterVerificationToken { get; set; }
    public virtual Token? ResetPasswordToken { get; set; }
    public virtual Token? RefreshToken { get; set; }

    public static void SetEntity(ModelBuilder builder)
    {
        ConfigureEntity(builder);
        SeedEntity(builder);
    }

    public static void ConfigureEntity(ModelBuilder builder)
    {
        builder.Entity<LoginData>(b =>
        {
            b.ToTable("logins_data");
            b.HasKey(x => x.Id);
            b.Property(x => x.AccountStatusId);
            b.Property(x => x.Email)
                .HasColumnName("email")
                .HasColumnType("varchar")
                .IsRequired()
                .HasComment("Email.")
                .HasMaxLength(64);
            b.Property(x => x.Salt)
                .HasColumnName("salt")
                .HasColumnType("varbinary")
                .HasComment("Salt.")
                .HasMaxLength(128);
            b.Property(x => x.PasswordHash)
                .HasColumnName("password_hash")
                .HasColumnType("varbinary")
                .HasComment("Password hash.")
                .HasMaxLength(64);
            b.Property(x => x.AccessKey)
                .HasColumnName("access_key")
                .HasColumnType("varbinary")
                .HasComment("Access key to encrypt storaged passwords with AES.")
                .HasMaxLength(21);
            b.Property(x => x.AccessSalt)
                .HasColumnName("access_salt")
                .HasColumnType("varbinary")
                .HasComment("Access salt to encrypt storaged passwords with AES.")
                .HasMaxLength(16);
            b.Property(x => x.AccessIV)
                .HasColumnName("access_iv")
                .HasColumnType("varbinary")
                .HasComment("Access IV to encrypt storaged passwords with AES.")
                .HasMaxLength(16);
            b.Property(x => x.PasswordLastChangeDate)
                .HasColumnName("password_last_change_date")
                .HasColumnType("datetime")
                .HasComment("Date of last password change.");
            b.Property(x => x.LoginAttempts)
                .HasColumnName("login_attempts")
                .HasColumnType("tinyint")
                .IsRequired()
                .HasComment("Count of login attempts.");
            b.HasOne(x => x.User)
                .WithOne(x => x.LoginData)
                .HasForeignKey<LoginData>(x => x.UserId)
                .IsRequired();
            b.HasOne(x => x.Role)
                .WithMany()
                .HasForeignKey(x => x.RoleId)
                .IsRequired();
            b.HasOne(x => x.AccountStatus)
                .WithMany()
                .HasForeignKey(x => x.AccountStatusId)
                .IsRequired();
            b.HasOne(x => x.RegisterVerificationToken)
                .WithOne()
                .HasForeignKey<LoginData>(x => x.RegisterVerificationTokenId);
            b.HasOne(x => x.ResetPasswordToken)
                .WithOne()
                .HasForeignKey<LoginData>(x => x.ResetPasswordTokenId);
            b.HasOne(x => x.RefreshToken)
                .WithOne()
                .HasForeignKey<LoginData>(x => x.RefreshTokenId);
        });
    }

    public static void SeedEntity(ModelBuilder builder)
    {
        builder.Entity<LoginData>().HasData
        (
            new LoginData
            {
                Id = 1,
                UserId = 1,
                LoginAttempts = 0,
                Email = "user1@gmail.com",
                RoleId = (byte)Entities.Role.User,
                Salt = ByteHelper.HexStringToByteArray("E6A4BE1133E97821E3FA9FDE8F07E5455733EA3382F591360E7A2D848ED69B2849607DDE5F39B101D13D9E93F93DE8BAB0C3B62FF4BD703EC9B42900AEE4BC7A5BB161C91B465B62FFB5B626459DEC8CFD50ECC77CA7DAC7735FDB0482F9BBD8D40AFBC195D9319A6BB92155183D6C3F203774546D7A75F7E5753020CC9D6D90"),
                PasswordHash = ByteHelper.HexStringToByteArray("3A019AAAD73D219399690BCC21FF5B097E1BCD85E58CEB058074594659A785AABE5E7BCADE39EB38B6A5805C6691878274C561F32C8F2E43D01F759635ED3D0B"),
                AccessIV = ByteHelper.HexStringToByteArray("44634E3D5E2367435D76343E2E5E4A34"),
                AccessKey = ByteHelper.HexStringToByteArray("2B722F70586E2E7E367D28733F66626D4C76384872"),
                AccessSalt = ByteHelper.HexStringToByteArray("3B5060217E5136214B356433317C673D"),
                AccountStatusId = (byte)AccountStatuses.Active,
                ResetPasswordTokenId = null,
                RegisterVerificationTokenId = null,
                RefreshTokenId = null,
                PasswordLastChangeDate = null
            },
            new LoginData
            {
                Id = 2,
                UserId = 2,
                LoginAttempts = 0,
                Email = "admin1@gmail.com",
                RoleId = (byte)Entities.Role.Admin,
                Salt = ByteHelper.HexStringToByteArray("E6A4BE1133E97821E3FA9FDE8F07E5455733EA3382F591360E7A2D848ED69B2849607DDE5F39B101D13D9E93F93DE8BAB0C3B62FF4BD703EC9B42900AEE4BC7A5BB161C91B465B62FFB5B626459DEC8CFD50ECC77CA7DAC7735FDB0482F9BBD8D40AFBC195D9319A6BB92155183D6C3F203774546D7A75F7E5753020CC9D6D90"),
                PasswordHash = ByteHelper.HexStringToByteArray("3A019AAAD73D219399690BCC21FF5B097E1BCD85E58CEB058074594659A785AABE5E7BCADE39EB38B6A5805C6691878274C561F32C8F2E43D01F759635ED3D0B"),
                AccessIV = ByteHelper.HexStringToByteArray("44634E3D5E2367435D76343E2E5E4A34"),
                AccessKey = ByteHelper.HexStringToByteArray("2B722F70586E2E7E367D28733F66626D4C76384872"),
                AccessSalt = ByteHelper.HexStringToByteArray("3B5060217E5136214B356433317C673D"),
                AccountStatusId = (byte)AccountStatuses.Active,
                ResetPasswordTokenId = null,
                RegisterVerificationTokenId = null,
                RefreshTokenId = null,
                PasswordLastChangeDate = null
            },
            new LoginData
            {
                Id = 3,
                UserId = 3,
                LoginAttempts = 0,
                Email = "worker1@gmail.com",
                RoleId = (byte)Entities.Role.Worker,
                Salt = ByteHelper.HexStringToByteArray("E6A4BE1133E97821E3FA9FDE8F07E5455733EA3382F591360E7A2D848ED69B2849607DDE5F39B101D13D9E93F93DE8BAB0C3B62FF4BD703EC9B42900AEE4BC7A5BB161C91B465B62FFB5B626459DEC8CFD50ECC77CA7DAC7735FDB0482F9BBD8D40AFBC195D9319A6BB92155183D6C3F203774546D7A75F7E5753020CC9D6D90"),
                PasswordHash = ByteHelper.HexStringToByteArray("3A019AAAD73D219399690BCC21FF5B097E1BCD85E58CEB058074594659A785AABE5E7BCADE39EB38B6A5805C6691878274C561F32C8F2E43D01F759635ED3D0B"),
                AccessIV = ByteHelper.HexStringToByteArray("44634E3D5E2367435D76343E2E5E4A34"),
                AccessKey = ByteHelper.HexStringToByteArray("2B722F70586E2E7E367D28733F66626D4C76384872"),
                AccessSalt = ByteHelper.HexStringToByteArray("3B5060217E5136214B356433317C673D"),
                AccountStatusId = (byte)AccountStatuses.Active,
                ResetPasswordTokenId = null,
                RegisterVerificationTokenId = null,
                RefreshTokenId = null,
                PasswordLastChangeDate = null
            },
            new LoginData
            {
                Id = 4,
                UserId = 4,
                LoginAttempts = 0,
                Email = "user2@gmail.com",
                RoleId = (byte)Entities.Role.User,
                Salt = ByteHelper.HexStringToByteArray("E6A4BE1133E97821E3FA9FDE8F07E5455733EA3382F591360E7A2D848ED69B2849607DDE5F39B101D13D9E93F93DE8BAB0C3B62FF4BD703EC9B42900AEE4BC7A5BB161C91B465B62FFB5B626459DEC8CFD50ECC77CA7DAC7735FDB0482F9BBD8D40AFBC195D9319A6BB92155183D6C3F203774546D7A75F7E5753020CC9D6D90"),
                PasswordHash = ByteHelper.HexStringToByteArray("3A019AAAD73D219399690BCC21FF5B097E1BCD85E58CEB058074594659A785AABE5E7BCADE39EB38B6A5805C6691878274C561F32C8F2E43D01F759635ED3D0B"),
                AccessIV = ByteHelper.HexStringToByteArray("44634E3D5E2367435D76343E2E5E4A34"),
                AccessKey = ByteHelper.HexStringToByteArray("2B722F70586E2E7E367D28733F66626D4C76384872"),
                AccessSalt = ByteHelper.HexStringToByteArray("3B5060217E5136214B356433317C673D"),
                AccountStatusId = (byte)AccountStatuses.Active,
                ResetPasswordTokenId = null,
                RegisterVerificationTokenId = null,
                RefreshTokenId = null,
                PasswordLastChangeDate = null
            },
            new LoginData
            {
                Id = 5,
                UserId = 5,
                LoginAttempts = 0,
                Email = "worker2@gmail.com",
                RoleId = (byte)Entities.Role.Worker,
                Salt = ByteHelper.HexStringToByteArray("E6A4BE1133E97821E3FA9FDE8F07E5455733EA3382F591360E7A2D848ED69B2849607DDE5F39B101D13D9E93F93DE8BAB0C3B62FF4BD703EC9B42900AEE4BC7A5BB161C91B465B62FFB5B626459DEC8CFD50ECC77CA7DAC7735FDB0482F9BBD8D40AFBC195D9319A6BB92155183D6C3F203774546D7A75F7E5753020CC9D6D90"),
                PasswordHash = ByteHelper.HexStringToByteArray("3A019AAAD73D219399690BCC21FF5B097E1BCD85E58CEB058074594659A785AABE5E7BCADE39EB38B6A5805C6691878274C561F32C8F2E43D01F759635ED3D0B"),
                AccessIV = ByteHelper.HexStringToByteArray("44634E3D5E2367435D76343E2E5E4A34"),
                AccessKey = ByteHelper.HexStringToByteArray("2B722F70586E2E7E367D28733F66626D4C76384872"),
                AccessSalt = ByteHelper.HexStringToByteArray("3B5060217E5136214B356433317C673D"),
                AccountStatusId = (byte)AccountStatuses.Active,
                ResetPasswordTokenId = null,
                RegisterVerificationTokenId = null,
                RefreshTokenId = null,
                PasswordLastChangeDate = null
            });
    }
}