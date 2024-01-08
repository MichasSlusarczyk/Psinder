using MediatR;
using Psinder.API.Domain.Models.Users;

namespace Psinder.API.Domain.Models.Shelters;

public class GetSheltersForWorkerMediatr : IRequest<GetSheltersResponse>
{
    public GetShelterRequest Request { get; set; }
}