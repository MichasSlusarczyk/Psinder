using MediatR;

namespace Psinder.API.Domain.Models.Users;

public class GetShelterMediatr : IRequest<GetShelterResponse>
{
    public GetShelterRequest Request { get; set; }
}
