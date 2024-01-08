using Psinder.DB.Domain.Entities;

namespace Psinder.DB.Domain.Repositories.Shelters;

public interface IShelterRepository
{
    Task<Shelter?> GetShelterById(long shelterId, CancellationToken cancellationToken);

    Task UpdateShelter(Shelter shelter, CancellationToken cancellationToken);

    Task<Shelter> AddShelter(Shelter shelter, CancellationToken cancellationToken);

    Task<List<long>?> GetShelterWorkersUserIds(long shelterId, CancellationToken cancellationToken);

    Task<List<Shelter>> GetAllShelters(CancellationToken cancellationToken);

    Task<List<Shelter>> GetAllSheltersForWorker(long workerId, CancellationToken cancellationToken);

    Task<bool> CheckWorkerAccessToShelter(long workerId, long shelterId, CancellationToken cancellationToken);

    Task<bool> CheckIfShelterExists(long shelterId, CancellationToken cancellationToken);
}