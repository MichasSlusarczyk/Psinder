using MediatR;

namespace Psinder.API.Domain.Models.Pets;

public class GetPetMediatr : IRequest<GetPetResponse>
{
    public GetPetMediatr(GetPetRequest request)
    {
        Request = request;
    }

    public GetPetRequest Request { get; set; }
}