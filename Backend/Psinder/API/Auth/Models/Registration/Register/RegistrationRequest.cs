using Microsoft.EntityFrameworkCore;
using Psinder.DB.Auth.Entities;
using Psinder.DB.Domain.Entities;

namespace Psinder.API.Auth.Models.Registrations;

public class RegistrationRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string RepeatPassword { get; set; }
    public Role Role { get; set; }
    public bool SignedForNewsletter { get; set; }
    public long? ShelterId { get; set; }

    public UserDetailsInfo? UserDetails{ get; set; }

    public class UserDetailsInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public UserGenders Gender { get; set; }
        public DateTime BirthDate { get; set; }
    }

    public User ToDomain()
    {
        var domainModel = new User()
        {
            SignedForNewsletter = SignedForNewsletter,
            LoginData = new LoginData()
            {
                Email = Email,
                RoleId = (byte)Role
            },
        };

        if (UserDetails is not null)
        {
            domainModel.UserDetails = new UserDetails()
            {
                FirstName = UserDetails.FirstName,
                LastName = UserDetails.LastName,
                PhoneNumber = UserDetails.PhoneNumber,
                City = UserDetails.City,
                PostalCode = UserDetails.PostalCode,
                Street = UserDetails.Street,
                StreetNumber = UserDetails.StreetNumber,
                Gender = UserDetails.Gender,
                BirthDate = UserDetails.BirthDate
            };
        }

        return domainModel;
    }
}
