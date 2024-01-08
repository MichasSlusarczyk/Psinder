using Psinder.DB.Domain.Entities;

namespace Psinder.API.Domain.Models.Users;

public class AddShelterRequest
{
    public string Name { get; set; }

    public string City { get; set; }

    public string Address { get; set; }

    public string Description { get; set; }

    public string PhoneNumber { get; set; }

    public string Email { get; set; }

    public static Shelter ToDomain(AddShelterRequest request)
    {
        return new Shelter()
        {
            Address = request.Address,
            City = request.City,
            Description = request.Description,
            Email = request.Email,
            Name = request.Name,
            PhoneNumber = request.PhoneNumber
        };
    }
}