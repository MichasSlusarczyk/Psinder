using MediatR;

namespace Psinder.API.Domain.Models.Appointments;

public class GetAllAppointmentsForShelterMediatr : IRequest<GetAllAppointmentsForShelterResponse>
{
    public GetAllAppointmentsForShelterRequest Request { get; set; }
}
