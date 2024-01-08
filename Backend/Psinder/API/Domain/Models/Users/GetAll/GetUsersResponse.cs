using Psinder.DB.Auth.Entities;
using Psinder.DB.Domain.Entities;

namespace Psinder.API.Domain.Models.Users;

public class GetUsersResponse
{
    public List<GetUsersRowResponse> Users {  get; set; }

    public class GetUsersRowResponse
    {
        public long Id { get; set; }
        public Role Role { get; set; }
        public bool SignedForNewsletter { get; set; }
        public string Email { get; set; }
        public AccountStatuses AccountStatus { get; set; }
        public long? ShelterId { get; set; }
        public GetUserDetailsRowResponse? UserDetails { get; set; }

        public class GetUserDetailsRowResponse
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
    }

    public static GetUsersResponse FromDomain(List<User> users, List<long?> sheltersId)
    {
        var result = new GetUsersResponse
        {
            Users = new List<GetUsersRowResponse>()
        };

        foreach (var user in users.Select((value, i) => new { i, value }))
        {
            var row = new GetUsersRowResponse()
            {
                Id = user.value.Id,
                SignedForNewsletter = user.value.SignedForNewsletter,
                Email = user.value.LoginData.Email,
                Role = (Role)user.value.LoginData.RoleId,
                AccountStatus = (AccountStatuses)user.value.LoginData.AccountStatusId,
                ShelterId = sheltersId.ElementAt(user.i)
            };

            if (user.value.UserDetails != null)
            {
                row.UserDetails = new GetUsersRowResponse.GetUserDetailsRowResponse()
                {
                    Id = user.value.UserDetails.Id,
                    LastName = user.value.UserDetails.LastName,
                    BirthDate = user.value.UserDetails.BirthDate,
                    City = user.value.UserDetails.City,
                    FirstName = user.value.UserDetails.FirstName,
                    Gender = user.value.UserDetails.Gender,
                    PhoneNumber = user.value.UserDetails.PhoneNumber,
                    PostalCode = user.value.UserDetails.PostalCode,
                    Street = user.value.UserDetails.Street,
                    StreetNumber = user.value.UserDetails.StreetNumber
                };
            }

            result.Users.Add(row);
        }

        return result;
    }
}
