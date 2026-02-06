using Service.Tasks.Data.Services;
using Service.Tasks.Domain.Models.User;

namespace Service.Tasks.Domain.Services.User;

public interface IUserManager
{
    Task<AuthenticationModel> Register(
        UserModel user,
        ITransaction? transaction = null,
        CancellationToken cancellationToken = default);

    Task<AuthenticationModel> Login(
        UserModel user,
        ITransaction? transaction = null,
        CancellationToken cancellationToken = default);

    Task<AuthenticationModel> Refresh(
        string refreshToken,
        ITransaction? transaction = null,
        CancellationToken cancellationToken = default);

    System.Threading.Tasks.Task ResetPassword(
        UserModel user,
        ITransaction? transaction = null,
        CancellationToken cancellationToken = default);
}
