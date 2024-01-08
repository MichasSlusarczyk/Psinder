namespace Psinder.API.Domain.Models.Users;

public class AddShelterWithUsersResponse
{
    public AddShelterWithUsersResponse(AddShelterWithUsersResponseStatus status, long? shelterId = null, string? errorMessage = null)
    {
        Status = status;
        ShelterId = shelterId;
        ErrorMessage = errorMessage;
    }

    public long? ShelterId { get; set; }

    public AddShelterWithUsersResponseStatus Status { get; set; }

    public string? ErrorMessage { get; set; }

    public static AddShelterWithUsersResponse Create(AddShelterWithUsersResponseStatus status, long? shelterId = null, string? errorMessage = null)
    {
        return new AddShelterWithUsersResponse(status, shelterId, errorMessage);
    }
}