using Autofac;
using Service.Tasks.Data.Profile;
using Service.Tasks.Data.Services;
using Sieve.Services;

namespace Service.Tasks.Data;

public sealed class TaskDataModule : Module
{
    protected override void Load(
        ContainerBuilder builder)
    {
        builder.RegisterType<SieveProcessor>()
            .As<ISieveProcessor>();

        builder.RegisterType<TransactionService>()
            .As<ITransactionService>();

        builder.RegisterType<ApplicationSieveProcessor>()
            .As<ISieveProcessor>()
            .SingleInstance();
    }
}
