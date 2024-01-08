using MediatR;
using Psinder.API.Domain.Models.Appointments;
using Psinder.DB.Domain.Entities;
using Psinder.DB.Domain.Repositories.Appointments;

namespace Psinder.API.Domain.Handlers;

public class AddAppointmentRequestHandler : IRequestHandler<AddAppointmentMediatr, Unit>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly ILogger<AddAppointmentRequestHandler> _logger;

    public AddAppointmentRequestHandler(
        IAppointmentRepository appointmentRepository,
        ILogger<AddAppointmentRequestHandler> logger)
    {
        _appointmentRepository = appointmentRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(AddAppointmentMediatr requestMediatr, CancellationToken cancellationToken)
    {
        try
        {
            var request = requestMediatr.Request;

            var domainModel = AddAppointmentRequest.ToDomain(request);
            domainModel.AppointmentStatus = AppointmentStatuses.Active;

            await _appointmentRepository.AddAppointment(domainModel, cancellationToken);

            return Unit.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}