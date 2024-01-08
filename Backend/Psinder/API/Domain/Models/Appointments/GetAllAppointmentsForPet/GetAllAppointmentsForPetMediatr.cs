using MediatR;

namespace Psinder.API.Domain.Models.Appointments;

public class GetAllAppointmentsForPetMediatr : IRequest<GetAllAppointmentsForPetResponse>
{
    public GetAllAppointmentsForPetRequest Request { get; set; }
}
