using System.Data.Common;

namespace Service.Tasks.Data.Services;

public interface ITransaction
    : IDisposable,
        IAsyncDisposable
{
    Task Commit(
        CancellationToken cancellationToken = default);

    Task Rollback(
        CancellationToken cancellationToken = default);

    public DbTransaction GetDbTransaction();
}
