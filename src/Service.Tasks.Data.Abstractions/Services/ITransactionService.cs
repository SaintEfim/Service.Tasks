namespace Service.Tasks.Data.Services;

public interface ITransactionService
{
    Task<TResult> Execute<TResult>(
        Func<ITransaction, CancellationToken, Task<TResult>> funcAsync,
        ITransaction? transaction = null,
        CancellationToken cancellationToken = default);
}
