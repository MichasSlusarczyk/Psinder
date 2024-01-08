namespace Psinder.API.Domain.Models.Pets;

public class DeletePetRequest
{
    public long PetId { get; set; }

    public DeletePetRequest(long petId)
    {
        PetId = petId;
    }
}
