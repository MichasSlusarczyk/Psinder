using Psinder.DB.Domain.Entities;

namespace Psinder.API.Domain.Models.Appointments;

public class GetAllAppointmentsForPetResponse
{
    public List<GetAllAppointmentsForPetRowResponse> Appointments { get; set; }

    public class GetAllAppointmentsForPetRowResponse
    {
        public long Id { get; set; }

        public long PetId { get; set; }

        public long UserId { get; set; }

        public DateTime AppointmentTimeStart { get; set; }

        public DateTime AppointmentTimeEnd { get; set; }

        public AppointmentStatuses AppointmentStatus { get; set; }
    }

    public static GetAllAppointmentsForPetResponse FromDomain(List<Appointment> appointments)
    {
        var result = new GetAllAppointmentsForPetResponse
        {
            Appointments = new List<GetAllAppointmentsForPetRowResponse>()
        };

        foreach (var appointment in appointments)
        {
            var row = new GetAllAppointmentsForPetRowResponse()
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