﻿using Psinder.DB.Domain.Entities;

namespace Psinder.API.Domain.Models.Shelters;

public class GetSheltersResponse
{
    public List<GetSheltersRowResponse> Shelters {  get; set; }

    public class GetSheltersRowResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }

    public static GetSheltersResponse FromDomain(List<Shelter> shelters)
    {
        var result = new GetSheltersResponse
        {
            Shelters = new List<GetSheltersRowResponse>()
        };

        foreach (var shelter in shelters)
        {
            var row = new GetSheltersRowResponse()
            {
                Id = shelter.Id,
                Address = shelter.Address,
                City = shelter.City,
                Description = shelter.Description,
                Email = shelter.Email,
                Name = shelter.Name,
                PhoneNumber = shelter.PhoneNumber
            };

            result.Shelters.Add(row);
        }

        return result;
    }
}
