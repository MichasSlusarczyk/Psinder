using MediatR;
using Psinder.API.Domain.Models.Appointments;
using Psinder.DB.Domain.Repositories.Appointments;

namespace Psinder.API.Domain.Handlers;

public class GetAllAppointmentsForPetRequestHandler : IRequestHandler<GetAllAppointmentsForPetMediatr, GetAllAppointmentsForPetResponse>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly ILogger<GetAllAppointmentsForPetRequestHandler> _logger;

    public GetAllAppointmentsForPetRequestHandler(
        IAppointmentRepository appointmentRepository,
        ILogger<GetAllAppointmentsForPetRequestHandler> logger)
    {
        _appointmentRepository = appointmentRepository;
        _logger = logger;
    }

    public async Task<GetAllAppointmentsForPetResponse> Handle(GetAllAppointmentsForPetMediatr requestMediatr, CancellationToken cancellationToken)
    {
        try
        {
            var request = requestMediatr.Request;

            var appointmentsData = await _appointmentRepository.GetAllAppointmentsForPetId(request.PetId, cancellationToken);

            return GetAllAppointmentsForPetResponse.FromDomain(appointmentsData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}