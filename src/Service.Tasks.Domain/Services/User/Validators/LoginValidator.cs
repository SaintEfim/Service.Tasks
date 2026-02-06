using FluentValidation;
using Service.Tasks.Domain.Models.Base.Validators;
using Service.Tasks.Domain.Models.User;

namespace Service.Tasks.Domain.Services.User.Validators;

internal sealed class LoginValidator
    : AbstractValidator<UserModel>,
        IDomainCustomValidator<UserModel>
{
    public LoginValidator(
        IUserProvider userProvider)
    {
        RuleFor(x => x)
            .CustomAsync(async (
                user,
                context,
                token) =>
            {
                var userExists = await userProvider.VerifyUser(user, cancellationToken: token);

                if (userExists == null)
                {
                    context.AddFailure(nameof(UserModel), "User does not exist");
                }
            });
    }

    public string ActionName => nameof(IUserManager.Login);
}
