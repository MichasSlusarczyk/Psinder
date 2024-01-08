using MediatR;

namespace Psinder.API.Domain.Models.Users;

public class AddShelterMediatr : IRequest<AddShelterResponse>
{
    public AddShelterRequest Request { get; set; }
}
