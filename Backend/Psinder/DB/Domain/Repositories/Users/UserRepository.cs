using Microsoft.EntityFrameworkCore;
using Psinder.DB.Auth.Entities;
using Psinder.DB.Common.Repositories.UnitOfWorks;
using Psinder.DB.Domain.Entities;

namespace Psinder.DB.Domain.Repositories.Users;

public class UserRepository : IUserRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public UserRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<User?> GetUserById(long userId, CancellationToken cancellationToken)
    {
        return await _unitOfWork.DatabaseContext.UsersEntity
            .Include(x => x.UserDetails)
            .Include(x => x.LoginData).ThenInclude(x => x.AccountStatus)
            .Include(x => x.LoginData).ThenInclude(x => x.Role)
            .SingleOrDefaultAsync(x => x.Id == userId, cancellationToken);
    }

    public async Task<List<User>> GetAllUsers(CancellationToken cancellationToken)
    {
        return await _unitOfWork.DatabaseContext.UsersEntity
            .Include(x => x.UserDetails)
            .Include(x => x.LoginData).ThenInclude(x => x.AccountStatus)
            .Include(x => x.LoginData).ThenInclude(x => x.Role)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateUser(User user, CancellationToken cancellationToken)
    {
        _unitOfWork.DatabaseContext.UsersEntity.Update(user);

        await _unitOfWork.DatabaseContext.SaveChangesAsync(cancellationToken);
        _unitOfWork.DatabaseContext.ChangeTracker.Clear();
    }

    public async Task<bool> CheckIfEmailIsAlreadyUsed(string email, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.DatabaseContext.LoginsDataEntity
            .AnyAsync(x => x.Email == email, cancellationToken);

        return result;
    }

    public async Task<bool> CheckIfUserExists(long userId, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.DatabaseContext.UsersEntity
            .AnyAsync(x => x.Id == userId, cancellationToken);

        return result;
    }

    public async Task<long?> GetUserIdByLoginId(long loginId, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.DatabaseContext.UsersEntity
            .Where(x => x.LoginData.Id == loginId)
            .Select(x => new
            {
                Id = x.Id,
            }).SingleOrDefaultAsync(cancellationToken);

        return result?.Id;
    }

    public async Task AddWorker(Worker worker, CancellationToken cancellationToken)
    {
        await _unitOfWork.DatabaseContext.AddAsync(worker);
        await _unitOfWork.DatabaseContext.SaveChangesAsync(cancellationToken);
        _unitOfWork.DatabaseContext.ChangeTracker.Clear();
    }

    public async Task<long?> GetWorkerShelterId(long userId, CancellationToken cancellationToken)
    {
        return (await _unitOfWork.DatabaseContext.WorkersEntity
            .Where(x => x.UserId == userId)
            .SingleOrDefaultAsync(cancellationToken))?.ShelterId;
    }

    public async Task DeleteWorkerInAllShelters(long userId, CancellationToken cancellationToken)
    {
        var workingPlacements = await _unitOfWork.DatabaseContext.WorkersEntity
            .Where(x => x.UserId == userId).ToListAsync(cancellationToken);

        if(workingPlacements.Any())
        {
            _unitOfWork.DatabaseContext.WorkersEntity.RemoveRange(workingPlacements);
            await _unitOfWork.DatabaseContext.SaveChangesAsync(cancellationToken);
            _unitOfWork.DatabaseContext.ChangeTracker.Clear();
        }
    }

    public async Task<RolesDictionary> GetRoleById(long roleId, CancellationToken cancellationToken)
    {
        return await _unitOfWork.DatabaseContext.RolesDictionaryEntity
            .Where(x => x.Id == roleId)
            .SingleAsync(cancellationToken);
    }
}