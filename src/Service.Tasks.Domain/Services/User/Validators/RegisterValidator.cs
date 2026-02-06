using FluentValidation;
using Service.Tasks.Domain.Models.Base.Validators;
using Service.Tasks.Domain.Models.User;
using Service.Tasks.Shared.Models;

namespace Service.Tasks.Domain.Services.User.Validators;

public class RegisterValidator
    : AbstractValidator<UserModel>,
        IDomainCustomValidator<UserModel>
{
    public RegisterValidator(
        IUserProvider userProvider)
    {
        RuleFor(x => x)
            .CustomAsync(async (
                user,
                context,
                token) =>
            {
                var userExists = (await userProvider.Get(
                    new FilterSettings { SearchText = $"UserName=={user.UserName}" },
                    cancellationToken: token)).Any();

                if (userExists)
                {
                    context.AddFailure(nameof(UserModel), "User already exists");
                }
            });
    }

    public string ActionName => nameof(IUserManager.Register);
}
