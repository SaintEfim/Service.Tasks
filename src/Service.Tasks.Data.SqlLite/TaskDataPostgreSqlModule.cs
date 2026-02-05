using Autofac;
using Microsoft.EntityFrameworkCore;
using Service.Tasks.Data.Repositories.Base;
using Service.Tasks.Data.SqlLite.Context;

namespace Service.Tasks.Data.SqlLite;

public sealed class PmServiceSitesDataPostgreSqlModule : Module
{
    protected override void Load(
        ContainerBuilder builder)
    {
        builder.RegisterModule<TaskDataModule>();

        builder.RegisterAssemblyTypes(ThisAssembly)
            .AsClosedTypesOf(typeof(IRepository<>))
            .AsImplementedInterfaces();

        builder.RegisterType<TaskDbContextFactory>()
            .AsSelf()
            .SingleInstance();

        builder.Register(c => c.Resolve<TaskDbContextFactory>()
                .CreateDbContext())
            .As<TaskDbContext>()
            .As<DbContext>()
            .InstancePerLifetimeScope();
    }
}
