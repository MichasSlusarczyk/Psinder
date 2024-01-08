using Microsoft.EntityFrameworkCore;

namespace Psinder.DB.Domain.Entities;

public class Appointment
{
    public long Id { get; set; }

    public long PetId { get; set; }

    public long UserId { get; set; }

    public DateTime AppointmentTimeStart { get; set; }

    public DateTime AppointmentTimeEnd { get; set; }

    public AppointmentStatuses AppointmentStatus { get; set; }

    public virtual Pet Pet { get; set; }

    public virtual User User { get; set; }

    public static void SetEntity(ModelBuilder builder)
    {
        ConfigureEntity(builder);
        SeedEntity(builder);
    }

    public static void ConfigureEntity(ModelBuilder builder)
    {
        builder.Entity<Appointment>(b =>
        {
            b.ToTable("appointments");
            b.HasKey(x => x.Id);
            b.Property(x => x.UserId);
            b.Property(x => x.PetId);
            b.Property(x => x.AppointmentTimeStart)
                .HasColumnName("appointment_time_start")
                .HasColumnType("datetime")
                .IsRequired()
                .HasComment("Appointment time start.");
            b.Property(x => x.AppointmentTimeEnd)
                .HasColumnName("appointment_time_end")
                .HasColumnType("datetime")
                .IsRequired()
                .HasComment("Appointment time end.");
            b.Property(x => x.AppointmentStatus)
                .HasColumnName("appointment_status")
                .HasColumnType("smallint")
                .IsRequired()
                .HasConversion(x => (int)x, c => (AppointmentStatuses)c)
                .HasComment("Appointment status.");
            b.HasOne(x => x.Pet)
                .WithMany(x => x.Appointments)
                .HasForeignKey(x => x.PetId);
            b.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);
        });
    }

    public static void SeedEntity(ModelBuilder builder)
    {
        builder.Entity<Appointment>().HasData
        (
            new Appointment()
            {
                Id = 1,
                AppointmentStatus = AppointmentStatuses.Active,
                AppointmentTimeStart = new DateTime(year: 2024, month: 1, day: 31, hour: 12, minute: 0, second: 0),
                AppointmentTimeEnd = new DateTime(year: 2024, month: 1, day: 31, hour: 12, minute: 30, second: 0),
                PetId = 1,
                UserId = 1
            },
            new Appointment()
            {
                Id = 2,
                AppointmentStatus = AppointmentStatuses.Active,
                AppointmentTimeStart = new DateTime(year: 2024, month: 1, day: 31, hour: 12, minute: 30, second: 0),
                AppointmentTimeEnd = new DateTime(year: 2024, month: 1, day: 31, hour: 13, minute: 0, second: 0),
                PetId = 1,
                UserId = 1
            },
            new Appointment()
            {
                Id = 3,
                AppointmentStatus = AppointmentStatuses.Cancelled,
                AppointmentTimeStart = new DateTime(year: 2024, month: 1, day: 31, hour: 12, minute: 30, second: 0),
                AppointmentTimeEnd = new DateTime(year: 2024, month: 1, day: 31, hour: 13, minute: 0, second: 0),
                PetId = 1,
                UserId = 1
            },
            new Appointment()
            {
                Id = 4,
                AppointmentStatus = AppointmentStatuses.Active,
                AppointmentTimeStart = new DateTime(year: 2024, month: 2, day: 5, hour: 10, minute: 30, second: 0),
                AppointmentTimeEnd = new DateTime(year: 2024, month: 2, day: 5, hour: 11, minute: 0, second: 0),
                PetId = 1,
                UserId = 4
            },
            new Appointment()
            {
                Id = 5,
                AppointmentStatus = AppointmentStatuses.Active,
                AppointmentTimeStart = new DateTime(year: 2024, month: 1, day: 31, hour: 9, minute: 0, second: 0),
                AppointmentTimeEnd = new DateTime(year: 2024, month: 1, day: 31, hour: 9, minute: 30, second: 0),
                PetId = 1,
                UserId = 4
            }
        );
    }
}