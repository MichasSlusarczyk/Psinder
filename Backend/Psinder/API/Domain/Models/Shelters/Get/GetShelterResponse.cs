using Psinder.DB.Domain.Entities;

namespace Psinder.API.Domain.Models.Users;

public class GetShelterResponse
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string City { get; set; }

    public string Address { get; set; }

    public string Description { get; set; }

    public string PhoneNumber { get; set; }

    public string Email { get; set; }

    public static GetShelterResponse FromDomain(Shelter shelter)
    {
        return new GetShelterResponse()
        {
            Id = shelter.Id,
            Address = shelter.Address,
            City = shelter.City,
            Description = shelter.Description,
            Email = shelter.Email,
            Name = shelter.Name,
            PhoneNumber = shelter.PhoneNumber
        };
    }
}