using MediatR;

namespace Psinder.API.Domain.Models.Pets;

public class UpdatePetMediatr : IRequest<Unit>
{
    public UpdatePetMediatr(UpdatePetRequest request)
    {
        Request = request;
    }

    public UpdatePetRequest Request { get; set; }
}
