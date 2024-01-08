using Psinder.DB.Domain.Entities;

namespace Psinder.API.Domain.Models.Appointments;

public class AddAppointmentRequest
{
    public long PetId { get; set; }

    public long UserId { get; set; }

    public DateTime AppointmentTimeStart { get; set; }

    public DateTime AppointmentTimeEnd { get; set; }

    public static Appointment ToDomain(AddAppointmentRequest request)
    {
        return new Appointment()
        {
            PetId = request.PetId,
            UserId = request.UserId,
            AppointmentTimeStart = request.AppointmentTimeStart,
            AppointmentTimeEnd = request.AppointmentTimeEnd
        };
    }
}

