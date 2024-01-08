using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Data.Common;

namespace Psinder.DB.Common.Repositories.UnitOfWorks;
public class UnitOfWork : IUnitOfWork
{
    private readonly DatabaseContext _databaseContext;

    public UnitOfWork(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public ValueTask DisposeAsync() => _databaseContext.DisposeAsync();
    public void Dispose() => _databaseContext.Dispose();

    public DbConnection Connection => _databaseContext.Database.GetDbConnection();
    public DbTransaction Transaction => _databaseContext.Database.CurrentTransaction?.GetDbTransaction();
    public DatabaseContext DatabaseContext => _databaseContext;

    public Task BeginTransaction(IsolationLevel isolationLevel, CancellationToken cancellationToken)
        => _databaseContext.Database.BeginTransactionAsync(isolationLevel, cancellationToken);

    public async Task CommitTransaction(CancellationToken cancellationToken)
    {
        await _databaseContext.SaveChangesAsync(cancellationToken);

        if (Transaction is not null)
        {
            await Transaction.CommitAsync(cancellationToken);
        }
    }

    public async Task RollbackTransaction(CancellationToken cancellationToken)
    {
        if (Transaction is not null)
        {
            await Transaction.RollbackAsync(cancellationToken);
        }
    }
}

