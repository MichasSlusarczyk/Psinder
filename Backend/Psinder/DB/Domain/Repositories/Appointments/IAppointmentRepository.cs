using Psinder.DB.Domain.Entities;

namespace Psinder.DB.Domain.Repositories.Appointments;

public interface IAppointmentRepository
{
    Task<Shelter?> GetAppointmentsByShelterId(long shelterId, CancellationToken cancellationToken);

    Task<List<Appointment>> GetAllAppointmentsForUserId(long userId, CancellationToken cancellationToken);

    Task<List<Appointment>> GetAllAppointmentsForPetId(long petId, CancellationToken cancellationToken);

    Task<long?> GetUserIdByAppointmentId(long appointmentId, CancellationToken cancellationToken);

    Task CancelAppointment(long appointmentId, CancellationToken cancellationToken);

    Task AddAppointment(Appointment appointment, CancellationToken cancellationToken);
}