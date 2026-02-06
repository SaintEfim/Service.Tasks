using Service.Tasks.Data.Services;
using Service.Tasks.Domain.Models.User;
using Service.Tasks.Domain.Services.Base;

namespace Service.Tasks.Domain.Services.User;

public interface IUserProvider : IDataProvider<UserModel>
{
    Task<UserModel?> VerifyUser(
        UserModel model,
        ITransaction? transaction = null,
        CancellationToken cancellationToken = default);
}
