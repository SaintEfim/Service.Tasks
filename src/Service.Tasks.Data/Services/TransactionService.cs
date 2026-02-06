using Microsoft.EntityFrameworkCore;
using Service.Tasks.Data.Extensions;

namespace Service.Tasks.Data.Services;

internal sealed class TransactionService : ITransactionService
{
    private readonly DbContext _context;

    public TransactionService(
        DbContext context)
    {
        _context = context;
    }

    public async Task<TResult> Execute<TResult>(
        Func<ITransaction, CancellationToken, Task<TResult>> funcAsync,
        ITransaction? transaction = null,
        CancellationToken cancellationToken = default)
    {
        var internalTransaction = false;
        if (transaction == null)
        {
            transaction = await _context.StartTransaction(cancellationToken);
            internalTransaction = true;
        }

        try
        {
            var result = await funcAsync(transaction, cancellationToken);
            if (internalTransaction)
            {
                await transaction.Commit(cancellationToken);
            }

            return result;
        }
        catch
        {
            if (internalTransaction)
            {
                await transaction.Rollback(cancellationToken);
            }

            throw;
        }
    }
}
