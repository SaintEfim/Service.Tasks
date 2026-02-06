using Autofac;
using Microsoft.Extensions.Configuration;
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

        builder.Register(c =>
            {
                var config = c.Resolve<IConfiguration>();
                return new AuthenticationSettings(config);
            })
            .SingleInstance();
    }
}
