using MediatR;

namespace Psinder.API.Domain.Models.Users;

public class UpdateShelterMediatr : IRequest<UpdateShelterResponse>
{
    public UpdateShelterRequest Request { get; set; }
}
