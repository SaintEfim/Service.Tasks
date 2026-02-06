using Autofac;
using Service.Tasks.UserHelpers.Helpers;

namespace Service.Tasks.UserHelpers;

public sealed class UserPasswordHasherModule : Module
{
    protected override void Load(
        ContainerBuilder builder)
    {
        builder.RegisterType<BCryptPasswordHasher>()
            .As<IBCryptPasswordHasher>()
            .InstancePerLifetimeScope();
    }
}
