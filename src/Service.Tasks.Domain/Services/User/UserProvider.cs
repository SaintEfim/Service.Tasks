using AutoMapper;
using Service.Tasks.Data.Models;
using Service.Tasks.Data.Repositories;
using Service.Tasks.Data.Services;
using Service.Tasks.Domain.Models.User;
using Service.Tasks.Domain.Services.Base;
using Service.Tasks.Shared.Models;
using Service.Tasks.UserHelpers.Helpers;

namespace Service.Tasks.Domain.Services.User;

public class UserProvider
    : DataProviderBase<UserModel, UserEntity, IUserRepository>,
        IUserProvider
{
    private readonly IBCryptPasswordHasher _passwordHasher;

    public UserProvider(
        IMapper mapper,
        IUserRepository repository,
        IBCryptPasswordHasher passwordHasher)
        : base(mapper, repository)
    {
        _passwordHasher = passwordHasher;
    }

    public async Task<UserModel?> VerifyUser(
        UserModel model,
        ITransaction? transaction = null,
        CancellationToken cancellationToken = default)
    {
        var users = await Repository.Get(new FilterSettings { SearchText = $"UserName=={model.UserName}" },
            transaction: transaction, cancellationToken: cancellationToken);
        var user = users.SingleOrDefault(x => _passwordHasher.Verify(model.Password, x.Password));

        return user == null ? null : Mapper.Map<UserModel>(user);
    }
}
