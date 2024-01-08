using MediatR;

namespace Psinder.API.Domain.Models.Pets;

public class DeletePetMediatr : IRequest<Unit>
{
    public DeletePetMediatr(DeletePetRequest request)
    {
        Request = request;
    }

    public DeletePetRequest Request { get; set; }
}
