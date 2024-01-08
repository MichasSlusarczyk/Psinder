using MediatR;

namespace Psinder.API.Domain.Models.Users;

public class AddShelterWithUsersMediatr : IRequest<AddShelterWithUsersResponse>
{
    public AddShelterWithUsersRequest Request { get; set; }
}