using MediatR;

namespace Psinder.API.Domain.Models.Pets;

public class AddPetMediatr : IRequest<AddPetResponse>
{
    public AddPetMediatr(AddPetRequest request)
    {
        Request = request;
    }

    public AddPetRequest Request { get; set; }
}
