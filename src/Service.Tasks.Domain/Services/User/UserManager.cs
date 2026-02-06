using AutoMapper;
using FluentValidation;
using Service.Tasks.Data.Models;
using Service.Tasks.Data.Repositories;
using Service.Tasks.Data.Services;
using Service.Tasks.Domain.Models.User;
using Service.Tasks.Domain.Services.Base;
using Service.Tasks.Shared.Models;
using Service.Tasks.UserHelpers.Helpers;
using Service.Tasks.UserJWTToken.Helpers;

namespace Service.Tasks.Domain.Services.User;

public class UserManager
    : ValidatorBase<UserModel>,
        IUserManager
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly ITransactionService _transactionService;
    private readonly IBCryptPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly AuthenticationSettings _authenticationSettings;

    public UserManager(
        IMapper mapper,
        IUserRepository userRepository,
        ITransactionService transactionService,
        IBCryptPasswordHasher passwordHasher,
        IEnumerable<IValidator> validators,
        IJwtTokenGenerator jwtTokenGenerator,
        AuthenticationSettings authenticationSettings)
        : base(validators)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _transactionService = transactionService;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _authenticationSettings = authenticationSettings;
    }

    public async Task<AuthenticationModel> Register(
        UserModel user,
        ITransaction? transaction = null,
        CancellationToken cancellationToken = default)
    {
        return await _transactionService.Execute(async (
            tr,
            token) =>
        {
            user.UserName = user.UserName.ToLower();
            user.Password = _passwordHasher.Hash(user.Password);

            await _userRepository.Create(_mapper.Map<UserEntity>(user), tr, token);

            return await Login(user, tr, token);
        }, transaction, cancellationToken);
    }

    public async Task<AuthenticationModel> Login(
        UserModel user,
        ITransaction? transaction = null,
        CancellationToken cancellationToken = default)
    {
        Validate(user, nameof(IUserManager.Login), cancellationToken);

        var userEntity =
            (await _userRepository.Get(new FilterSettings { SearchText = $"UserName=={user.UserName.ToLower()}" },
                transaction: transaction, cancellationToken: cancellationToken)).Single();

        var accessToken = await _jwtTokenGenerator.GenerateToken(userEntity.Id.ToString(), userEntity.Role,
            _authenticationSettings.AccessSecretKey, TimeSpan.Parse(_authenticationSettings.AccessHours),
            cancellationToken);
        var refreshToken = await _jwtTokenGenerator.GenerateToken(userEntity.Id.ToString(), userEntity.Role,
            _authenticationSettings.RefreshSecretKey, TimeSpan.Parse(_authenticationSettings.RefreshHours),
            cancellationToken);

        return new AuthenticationModel
        {
            TokenType = "Bearer",
            AccessToken = accessToken,
            ExpiresIn = (int) TimeSpan.Parse(_authenticationSettings.AccessHours)
                .TotalSeconds,
            RefreshToken = refreshToken
        };
    }

    public async Task<AuthenticationModel> Refresh(
        string refreshToken,
        ITransaction? transaction = null,
        CancellationToken cancellationToken = default)
    {
        var (userId, userRole) = await _jwtTokenGenerator.DecodeRefreshToken(refreshToken,
            _authenticationSettings.RefreshSecretKey, cancellationToken);

        if (!Enum.TryParse(userRole, out RoleEnum enumRole))
        {
            throw new InvalidDataException("Invalid user role");
        }

        var newAccessToken = await _jwtTokenGenerator.GenerateToken(userId, enumRole,
            _authenticationSettings.AccessSecretKey, TimeSpan.Parse(_authenticationSettings.AccessHours),
            cancellationToken);
        var newRefreshToken = await _jwtTokenGenerator.GenerateToken(userId, enumRole,
            _authenticationSettings.RefreshSecretKey, TimeSpan.Parse(_authenticationSettings.RefreshHours),
            cancellationToken);

        return new AuthenticationModel
        {
            TokenType = "Bearer",
            AccessToken = newAccessToken,
            ExpiresIn = (int) TimeSpan.Parse(_authenticationSettings.AccessHours)
                .TotalSeconds,
            RefreshToken = newRefreshToken
        };
    }

    public async System.Threading.Tasks.Task ResetPassword(
        Guid userId,
        string newPassword,
        ITransaction? transaction = null,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetOneById(userId, transaction: transaction,
            cancellationToken: cancellationToken);

        user.Password = _passwordHasher.Hash(newPassword);

        await _userRepository.Update(user, transaction, cancellationToken);
    }
}
