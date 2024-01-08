using Psinder.DB.Auth.Entities;
using Psinder.DB.Domain.Entities;

namespace Psinder.API.Domain.Models.Users;

public class GetUserResponse
{
    public long Id { get; set; }
    public Role Role { get; set; }
    public bool SignedForNewsletter { get; set; }
    public string Email { get; set; }
    public AccountStatuses AccountStatus { get; set; }
    public GetUserDetailsResponse? UserDetails { get; set; }
    public long? ShelterId { get; set; }

    public class GetUserDetailsResponse
    {
        public long Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? Street { get; set; }
        public string? StreetNumber { get; set; }
        public UserGenders? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
    }

    public static GetUserResponse FromDomain(User user, long? shelterId)
    {
        var result = new GetUserResponse()
        {
            Id = user.Id,
            Email = user.LoginData.Email,
            SignedForNewsletter = user.SignedForNewsletter,
            Role = (Role)user.LoginData.RoleId,
            AccountStatus = (AccountStatuses)user.LoginData.AccountStatusId,
            ShelterId = shelterId
        };

        if(user.UserDetails != null)
        {
            result.UserDetails = new GetUserDetailsResponse()
            {
                Id = user.UserDetails.Id,
                LastName = user.UserDetails.LastName,
                BirthDate = user.UserDetails.BirthDate,
                City = user.UserDetails.City,
                FirstName = user.UserDetails.FirstName,
                Gender = user.UserDetails.Gender,
                PhoneNumber = user.UserDetails.PhoneNumber,
                PostalCode = user.UserDetails.PostalCode,
                Street = user.UserDetails.Street,
                StreetNumber = user.UserDetails.StreetNumber
            };
        }

        return result;
    }
}
