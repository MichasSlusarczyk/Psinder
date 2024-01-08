using Psinder.DB.Auth.Entities;
using Psinder.DB.Domain.Entities;

namespace Psinder.API.Domain.Models.Users;

public class AddShelterWithUsersRequest
{
    public string Name { get; set; }

    public string City { get; set; }

    public string Address { get; set; }

    public string Description { get; set; }

    public string PhoneNumber { get; set; }

    public string Email { get; set; }

    public List<WorkerInfo> Workers { get; set; }

    public class WorkerInfo
    {
        public string Email { get; set; }
        public WorkerDetailsInfo? WorkerDetails { get; set; }

        public class WorkerDetailsInfo
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
    }

    public static Shelter ToDomain(AddShelterWithUsersRequest request)
    {
        var shelter = new Shelter()
        {
            Address = request.Address,
            City = request.City,
            Description = request.Description,
            Email = request.Email,
            Name = request.Name,
            PhoneNumber = request.PhoneNumber,
            Workers = new List<Worker>()
        };

        foreach (var worker in request.Workers)
        {
            var user = new User()
            {
                SignedForNewsletter = false,
                LoginData = new LoginData()
                {
                    Email = request.Email,
                    RoleId = (byte)Role.Worker
                }
            };

            if (worker.WorkerDetails is not null)
            {
                user.UserDetails = new UserDetails()
                {
                    FirstName = worker.WorkerDetails.FirstName,
                    LastName = worker.WorkerDetails.LastName,
                    PhoneNumber = worker.WorkerDetails.PhoneNumber,
                    City = worker.WorkerDetails.City,
                    PostalCode = worker.WorkerDetails.PostalCode,
                    Street = worker.WorkerDetails.Street,
                    StreetNumber = worker.WorkerDetails.StreetNumber,
                    Gender = worker.WorkerDetails.Gender,
                    BirthDate = worker.WorkerDetails.BirthDate
                };
            }

            shelter.Workers.Add(
                new Worker() { User = user }
            );
        }

        return shelter;
    }
}
