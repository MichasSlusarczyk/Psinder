using Psinder.DB.Auth.Entities;
using Psinder.DB.Domain.Entities;

namespace Psinder.API.Domain.Models.Users;

public class UpdateUserRequest
{
    public long Id { get; set; }
    public bool? SignedForNewsletter { get; set; }
    public AccountStatuses? AccountStatus { get; set; }
    public Role? Role { get; set; }
    public long? ShelterId { get; set; }
    public UpdateUserDetailsRequest? UserDetails { get; set; }

    public class UpdateUserDetailsRequest
    {
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
}
