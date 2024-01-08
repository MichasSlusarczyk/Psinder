using Psinder.DB.Auth.Entities;
using Psinder.DB.Domain.Entities;

namespace Psinder.DB.Domain.Repositories.Users;

public interface IUserRepository
{
    Task<User?> GetUserById(long userId, CancellationToken cancellationToken);
    Task<List<User>> GetAllUsers(CancellationToken cancellationToken);
    Task UpdateUser(User user, CancellationToken cancellationToken);
    Task<bool> CheckIfEmailIsAlreadyUsed(string email, CancellationToken cancellationToken);
    Task<bool> CheckIfUserExists(long userId, CancellationToken cancellationToken);
    Task<long?> GetUserIdByLoginId(long loginId, CancellationToken cancellationToken);
    Task AddWorker(Worker worker, CancellationToken cancellationToken);
    Task<long?> GetWorkerShelterId(long userId, CancellationToken cancellationToken);
    Task DeleteWorkerInAllShelters(long userId, CancellationToken cancellationToken);
    Task<RolesDictionary> GetRoleById(long roleId, CancellationToken cancellationToken);

}
