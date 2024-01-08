using MediatR;
using Psinder.API.Domain.Models.Users;
using Psinder.DB.Domain.Repositories.Users;

namespace Psinder.API.Domain.Handlers;

public class GetUsersRequestHandler : IRequestHandler<GetUsersMediatr, GetUsersResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<GetUserRequestHandler> _logger;

    public GetUsersRequestHandler(
        IUserRepository userRepository,
        ILogger<GetUserRequestHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<GetUsersResponse> Handle(GetUsersMediatr requestMediatr, CancellationToken cancellationToken)
    {
        try
        {
            var usersData = await _userRepository.GetAllUsers(cancellationToken);

            var sheltersId = new List<long?>();
            foreach (var user in usersData)
            {
                sheltersId.Add(await _userRepository.GetWorkerShelterId(user.Id, cancellationToken));
            }

            return GetUsersResponse.FromDomain(usersData, sheltersId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}