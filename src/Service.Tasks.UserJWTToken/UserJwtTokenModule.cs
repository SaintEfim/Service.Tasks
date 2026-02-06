using Autofac;
using Service.Tasks.UserJWTToken.Helpers;

namespace Service.Tasks.UserJWTToken;

public class UserJwtTokenModule : Module
{
    protected override void Load(
        ContainerBuilder builder)
    {
        builder.RegisterType<JwtTokenGenerator>()
            .As<IJwtTokenGenerator>()
            .InstancePerLifetimeScope();
    }
}
