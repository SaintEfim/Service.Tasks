using Autofac;
using Microsoft.Extensions.Configuration;
using Service.Tasks.UserJwtToken.Helpers;

namespace Service.Tasks.UserJwtToken;

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
