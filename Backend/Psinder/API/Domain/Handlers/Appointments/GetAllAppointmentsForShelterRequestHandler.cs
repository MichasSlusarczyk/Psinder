using MediatR;
using Psinder.API.Common;
using Psinder.API.Common.Exceptions;
using Psinder.API.Domain.Models.Appointments;
using Psinder.DB.Domain.Repositories.Appointments;
using Psinder.DB.Domain.Repositories.Shelters;

namespace Psinder.API.Domain.Handlers;

public class GetAllAppointmentsForShelterRequestHandler : IRequestHandler<GetAllAppointmentsForShelterMediatr, GetAllAppointmentsForShelterResponse>
{
    private readonly IIdentityService _identityService;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IShelterRepository _shelterRepository;
    private readonly ILogger<GetAllAppointmentsForShelterRequestHandler> _logger;

    public GetAllAppointmentsForShelterRequestHandler(
        IIdentityService identityService,
        IAppointmentRepository appointmentRepository,
        IShelterRepository shelterRepository,
        ILogger<GetAllAppointmentsForShelterRequestHandler> logger)
    {
        _identityService = identityService;
        _appointmentRepository = appointmentRepository;
        _shelterRepository = shelterRepository;
        _logger = logger;
    }

    public async Task<GetAllAppointmentsForShelterResponse> Handle(GetAllAppointmentsForShelterMediatr requestMediatr, CancellationToken cancellationToken)
    {
        try
        {
            var request = requestMediatr.Request;

            var usersIds = await _shelterRepository.GetShelterWorkersUserIds(request.ShelterId, cancellationToken) 
                ?? new List<long>();

            if (!_identityService.VerifyAccessForAdmin(usersIds))
            {
                throw new AccessDeniedException($"For a logged in user with role: {_identityService.CurrentUserRole} and ID: {_identityService.CurrentUserId} no access to data of shelter with ID: {request.ShelterId}.");
            }

            var shelterData = await _appointmentRepository.GetAppointmentsByShelterId(request.ShelterId, cancellationToken)
                ?? throw new Exception($"No shelter data found for ID: {request.ShelterId}.");

            return GetAllAppointmentsForShelterResponse.FromDomain(shelterData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}