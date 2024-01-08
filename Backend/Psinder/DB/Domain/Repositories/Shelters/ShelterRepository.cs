using Microsoft.EntityFrameworkCore;
using Psinder.DB.Common.Repositories.UnitOfWorks;
using Psinder.DB.Domain.Entities;

namespace Psinder.DB.Domain.Repositories.Shelters;

public class ShelterRepository : IShelterRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public ShelterRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Shelter?> GetShelterById(long shelterId, CancellationToken cancellationToken)
    {
        return await _unitOfWork.DatabaseContext.SheltersEntity
            .SingleOrDefaultAsync(x => x.Id == shelterId, cancellationToken);
    }

    public async Task UpdateShelter(Shelter shelter, CancellationToken cancellationToken)
    {
        _unitOfWork.DatabaseContext.SheltersEntity.Update(shelter);

        await _unitOfWork.DatabaseContext.SaveChangesAsync(cancellationToken);
        _unitOfWork.DatabaseContext.ChangeTracker.Clear();
    }

    public async Task<Shelter> AddShelter(Shelter shelter, CancellationToken cancellationToken)
    {
        await _unitOfWork.DatabaseContext.AddAsync(shelter);
        await _unitOfWork.DatabaseContext.SaveChangesAsync(cancellationToken);
        _unitOfWork.DatabaseContext.ChangeTracker.Clear();

        return shelter;
    }

    public async Task<List<long>?> GetShelterWorkersUserIds(long shelterId, CancellationToken cancellationToken)
    {
        return await _unitOfWork.DatabaseContext.WorkersEntity.Where(x => x.ShelterId == shelterId).Select(x => x.UserId).ToListAsync(cancellationToken);
    }

    public async Task<List<Shelter>> GetAllShelters(CancellationToken cancellationToken)
    {
        return await _unitOfWork.DatabaseContext.SheltersEntity
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Shelter>> GetAllSheltersForWorker(long workerId, CancellationToken cancellationToken)
    {
        return await _unitOfWork.DatabaseContext.SheltersEntity
            .Where(x => x.Workers.Any(x => x.UserId == workerId))
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> CheckWorkerAccessToShelter(long workerId, long shelterId, CancellationToken cancellationToken)
    {
        return await _unitOfWork.DatabaseContext.WorkersEntity
            .AnyAsync(x => x.UserId == workerId && x.ShelterId == shelterId, cancellationToken);
    }

    public async Task<bool> CheckIfShelterExists(long shelterId, CancellationToken cancellationToken)
    {
        return await _unitOfWork.DatabaseContext.WorkersEntity
            .AnyAsync(x => x.ShelterId == shelterId, cancellationToken);
    }
}
