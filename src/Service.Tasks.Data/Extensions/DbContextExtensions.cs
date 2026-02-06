using Microsoft.EntityFrameworkCore;
using Service.Tasks.Data.Services;

namespace Service.Tasks.Data.Extensions;

public static class DbContextExtensions
{
    public static async Task UseTransaction(
        this DbContext context,
        ITransaction transaction,
        CancellationToken cancellationToken)
    {
        await context.Database.UseTransactionAsync(transaction.GetDbTransaction(), cancellationToken);
    }

    public static async Task<ITransaction> StartTransaction(
        this DbContext context,
        CancellationToken cancellationToken = default)
    {
        var dbContextTransaction = await context.Database.BeginTransactionAsync(cancellationToken);
        return new Transaction(dbContextTransaction);
    }
}
