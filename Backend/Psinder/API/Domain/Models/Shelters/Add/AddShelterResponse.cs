namespace Psinder.API.Domain.Models.Users;

public class AddShelterResponse
{
    public AddShelterResponse(AddShelterStatus status, long? shelterId)
    {
        Status = status;
        ShelterId = shelterId;
    }

    public long? ShelterId { get; set; }

    public AddShelterStatus Status { get; set; }

    public static AddShelterResponse Create(AddShelterStatus status, long? shelterId = null)
    {
        return new AddShelterResponse(status, shelterId);
    }
}