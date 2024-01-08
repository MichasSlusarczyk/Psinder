using Psinder.DB.Auth.Entities;
using Psinder.DB.Domain.Entities;

namespace Psinder.API.Domain.Models.Users;

public class UpdateUserResponse
{
    public long Id { get; set; }
    public Role Role { get; set; }
    public bool SignedForNewsletter { get; set; }
    public AccountStatuses AccountStatus { get; set; }
    public UpdateUserDetailsResponse? UserDetails { get; set; }

    public class UpdateUserDetailsResponse
    {
        public long Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? Street { get; set; }
        public string? StreetNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public UserGenders? Gender { get; set; }
    }

    public static UpdateUserResponse FromDomain(User user)
    {
        var result = new UpdateUserResponse()
        {
            Id = user.Id,
            SignedForNewsletter = user.SignedForNewsletter,
            Role = (Role)user.LoginData.RoleId,
            AccountStatus = (AccountStatuses)user.LoginData.AccountStatusId
        };

        if(user.UserDetails != null)
        {
            result.UserDetails = new UpdateUserDetailsResponse()
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
