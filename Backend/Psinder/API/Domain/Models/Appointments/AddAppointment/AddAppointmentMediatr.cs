using MediatR;

namespace Psinder.API.Domain.Models.Appointments;

public class AddAppointmentMediatr : IRequest<Unit>
{
    public AddAppointmentRequest Request { get; set; }
}
