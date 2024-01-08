using Psinder.DB.Domain.Entities;

namespace Psinder.API.Domain.Models.Appointments;

public class GetAllAppointmentsForShelterResponse
{
    public long ShelterId { get; set; }

    public List<ShelterPetInfo> Pets { get; set; }

    public class ShelterPetInfo
    {
        public long PetId { get; set; }

        public List<ShelterPetAppointmentInfo> Appointments { get; set; }
    }

    public class ShelterPetAppointmentInfo
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public DateTime AppointmentTimeStart { get; set; }

        public DateTime AppointmentTimeEnd { get; set; }

        public AppointmentStatuses AppointmentStatus { get; set; }
    }

    public static GetAllAppointmentsForShelterResponse FromDomain(Shelter shelter)
    {
        var shelterModel = new GetAllAppointmentsForShelterResponse()
        {
            Pets = new List<ShelterPetInfo>()
        };

        foreach (var pet in shelter.Pets)
        {
            var petModel = new ShelterPetInfo()
            {
                PetId = pet.Id,
                Appointments = new List<ShelterPetAppointmentInfo>()            
            };

            foreach (var appointment in pet.Appointments)
            {
                var appointmentModel = new ShelterPetAppointmentInfo()
                {
                    AppointmentStatus = appointment.AppointmentStatus,
                    AppointmentTimeStart = appointment.AppointmentTimeStart,
                    AppointmentTimeEnd = appointment.AppointmentTimeEnd,
                    Id = appointment.Id,
                    UserId = appointment.UserId
                };

                petModel.Appointments.Add(appointmentModel);
            }

            shelterModel.Pets.Add(petModel);
        }

        return shelterModel;
    }
}