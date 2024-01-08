using MediatR;

namespace Psinder.API.Domain.Models.Appointments;

public class GetAllAppointmentsForUserMediatr : IRequest<GetAllAppointmentsForUserResponse>
{
    public GetAllAppointmentsForUserRequest Request { get; set; }
}
