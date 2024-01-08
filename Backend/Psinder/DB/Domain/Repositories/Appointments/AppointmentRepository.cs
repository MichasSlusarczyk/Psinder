using Microsoft.EntityFrameworkCore;
using Psinder.DB.Common.Repositories.UnitOfWorks;
using Psinder.DB.Domain.Entities;

namespace Psinder.DB.Domain.Repositories.Appointments;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public AppointmentRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Shelter?> GetAppointmentsByShelterId(long shelterId, CancellationToken cancellationToken)
    {
        return await _unitOfWork.DatabaseContext.SheltersEntity
            .Include(x => x.Pets)
            .ThenInclude(x => x.Appointments)
            .SingleOrDefaultAsync(x => x.Id == shelterId, cancellationToken);
    }

    public async Task<List<Appointment>> GetAllAppointmentsForUserId(long userId, CancellationToken cancellationToken)
    {
        return await _unitOfWork.DatabaseContext.AppointmentsEntity
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Appointment>> GetAllAppointmentsForPetId(long petId, CancellationToken cancellationToken)
    {
        return await _unitOfWork.DatabaseContext.AppointmentsEntity
            .Where(x => x.PetId == petId)
            .ToListAsync(cancellationToken);
    }

    public async Task<long?> GetUserIdByAppointmentId(long appointmentId, CancellationToken cancellationToken)
    {
        return await _unitOfWork.DatabaseContext.AppointmentsEntity
            .Where(x => x.Id == appointmentId)
            .Select(x => x.UserId)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task CancelAppointment(long appointmentId, CancellationToken cancellationToken)
    {
        var appointment = await _unitOfWork.DatabaseContext.AppointmentsEntity
            .SingleAsync(x => x.Id == appointmentId, cancellationToken);

        appointment.AppointmentStatus = AppointmentStatuses.Cancelled;

        _unitOfWork.DatabaseContext.Update(appointment);

        await _unitOfWork.DatabaseContext.SaveChangesAsync(cancellationToken);
        _unitOfWork.DatabaseContext.ChangeTracker.Clear();
    }

    public async Task AddAppointment(Appointment appointment, CancellationToken cancellationToken)
    {
        await _unitOfWork.DatabaseContext.AddAsync(appointment);
        await _unitOfWork.DatabaseContext.SaveChangesAsync(cancellationToken);
        _unitOfWork.DatabaseContext.ChangeTracker.Clear();
    }
}