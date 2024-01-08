using Microsoft.EntityFrameworkCore;
using Psinder.DB.Common.Repositories.UnitOfWorks;

namespace Psinder.IntegrationTests;

public class Fixtures
{
    private readonly IUnitOfWork _unitOfWork;

    public Fixtures(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Add<T>(T obj)
    {
        await _unitOfWork.DatabaseContext.AddAsync(obj);
        await SaveChangesWithIdentityInsertAsync<T>();
    }

    public async Task<int> SaveChangesWithIdentityInsertAsync<T>(CancellationToken token = default)
    {
        await using var transaction = await _unitOfWork.DatabaseContext.Database.BeginTransactionAsync(token);
        await SetIdentityInsertAsync<T>(true, token);
        int ret = await _unitOfWork.DatabaseContext.SaveChangesAsync(token);
        await SetIdentityInsertAsync<T>(false, token);
        await transaction.CommitAsync(token);

        return ret;
    }

    private async Task SetIdentityInsertAsync<T>(bool enable, CancellationToken token)
    {
        var entityType = _unitOfWork.DatabaseContext.Model.FindEntityType(typeof(T));
        var value = enable ? "ON" : "OFF";
        string query = $"SET IDENTITY_INSERT {entityType.GetSchema()}.{entityType.GetTableName()} {value}";
        await _unitOfWork.DatabaseContext.Database.ExecuteSqlRawAsync(query, token);
    }
}
