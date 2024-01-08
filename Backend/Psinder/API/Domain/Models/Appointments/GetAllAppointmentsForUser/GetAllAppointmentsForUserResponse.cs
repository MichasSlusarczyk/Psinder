using Psinder.DB.Domain.Entities;

namespace Psinder.API.Domain.Models.Appointments;

public class GetAllAppointmentsForUserResponse
{
    public List<GetAllAppointmentsForUserRowResponse> Appointments { get; set; }

    public class GetAllAppointmentsForUserRowResponse
    {
        public long Id { get; set; }

        public long PetId { get; set; }

        public long UserId { get; set; }

        public DateTime AppointmentTimeStart { get; set; }

        public DateTime AppointmentTimeEnd { get; set; }

        public AppointmentStatuses AppointmentStatus { get; set; }
    }

    public static GetAllAppointmentsForUserResponse FromDomain(List<Appointment> appointments)
    {
        var result = new GetAllAppointmentsForUserResponse
        {
            Appointments = new List<GetAllAppointmentsForUserRowResponse>()
        };

        foreach (var appointment in appointments)
        {
            var row = new GetAllAppointmentsForUserRowResponse()
            {
                AppointmentStatus = appointment.AppointmentStatus,
                AppointmentTimeStart = appointment.AppointmentTimeStart,
                AppointmentTimeEnd = appointment.AppointmentTimeEnd,
                Id = appointment.Id,
                PetId = appointment.PetId,
                UserId = appointment.UserId
            };

            result.Appointments.Add(row);
        }

        return result;
    }
}