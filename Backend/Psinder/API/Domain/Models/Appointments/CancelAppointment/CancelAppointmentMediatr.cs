using MediatR;

namespace Psinder.API.Domain.Models.Appointments;

public class CancelAppointmentMediatr : IRequest<Unit>
{
    public CancelAppointmentRequest Request { get; set; }
}
