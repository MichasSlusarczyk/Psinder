using MediatR;
using Psinder.API.Common;
using Psinder.API.Common.Exceptions;
using Psinder.API.Domain.Models.Appointments;
using Psinder.DB.Domain.Repositories.Appointments;

namespace Psinder.API.Domain.Handlers;

public class CancelAppointmentRequestHandler : IRequestHandler<CancelAppointmentMediatr, Unit>
{
    private readonly IIdentityService _identityService;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly ILogger<CancelAppointmentRequestHandler> _logger;

    public CancelAppointmentRequestHandler(
        IIdentityService identityService,
        IAppointmentRepository appointmentRepository,
        ILogger<CancelAppointmentRequestHandler> logger)
    {
        _identityService = identityService;
        _appointmentRepository = appointmentRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(CancelAppointmentMediatr requestMediatr, CancellationToken cancellationToken)
    {
        try
        {
            var request = requestMediatr.Request;

            var userId = await _appointmentRepository.GetUserIdByAppointmentId(request.Id, cancellationToken)
                ?? throw new Exception($"User ID for appointment to cancel with ID: {request.Id} not found.");

            if (!_identityService.VerifyAccessForWorkerAndAdmin(userId))
            {
                throw new AccessDeniedException($"For a logged in user with role: {_identityService.CurrentUserRole} and ID: {_identityService.CurrentUserId} no access to data of user with ID: {userId}.");
            }

            await _appointmentRepository.CancelAppointment(request.Id, cancellationToken);

            return new Unit();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}