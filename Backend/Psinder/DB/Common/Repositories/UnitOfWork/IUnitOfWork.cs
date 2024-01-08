using System.Data;
using System.Data.Common;

namespace Psinder.DB.Common.Repositories.UnitOfWorks;

public interface IUnitOfWork : IAsyncDisposable, IDisposable
{
    DbConnection Connection { get; }
    DbTransaction Transaction { get; }
    DatabaseContext DatabaseContext { get; }

    Task BeginTransaction(IsolationLevel isolationLevel, CancellationToken cancellationToken);
    Task CommitTransaction(CancellationToken cancellationToken);
    Task RollbackTransaction(CancellationToken cancellationToken);
}

