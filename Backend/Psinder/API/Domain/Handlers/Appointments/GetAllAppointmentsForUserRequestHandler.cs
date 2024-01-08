using MediatR;
using Psinder.API.Common;
using Psinder.API.Common.Exceptions;
using Psinder.API.Domain.Models.Appointments;
using Psinder.DB.Domain.Repositories.Appointments;

namespace Psinder.API.Domain.Handlers;

public class GetAllAppointmentsForUserRequestHandler : IRequestHandler<GetAllAppointmentsForUserMediatr, GetAllAppointmentsForUserResponse>
{
    private readonly IIdentityService _identityService;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly ILogger<GetAllAppointmentsForUserRequestHandler> _logger;

    public GetAllAppointmentsForUserRequestHandler(
        IIdentityService identityService,
        IAppointmentRepository appointmentRepository,
        ILogger<GetAllAppointmentsForUserRequestHandler> logger)
    {
        _identityService = identityService;
        _appointmentRepository = appointmentRepository;
        _logger = logger;
    }

    public async Task<GetAllAppointmentsForUserResponse> Handle(GetAllAppointmentsForUserMediatr requestMediatr, CancellationToken cancellationToken)
    {
        try
        {
            var request = requestMediatr.Request;

            if (!_identityService.VerifyAccessForWorkerAndAdmin(request.UserId))
            {
                throw new AccessDeniedException($"For a logged in user with role: {_identityService.CurrentUserRole} and ID: {_identityService.CurrentUserId} no access to data for user with ID: {request.UserId}.");
            }

            var userData = await _appointmentRepository.GetAllAppointmentsForUserId(request.UserId, cancellationToken)
                ?? throw new Exception($"No appointments data found for user ID: {request.UserId}.");

            return GetAllAppointmentsForUserResponse.FromDomain(userData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}