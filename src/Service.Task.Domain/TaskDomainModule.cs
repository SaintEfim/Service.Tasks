using Autofac;
using Service.Task.Domain.Services.Base;
using Service.Tasks.Data.SqlLite;

namespace Service.Task.Domain;

public class TaskDomainModule : Module
{
    protected override void Load(
        ContainerBuilder builder)
    {
        builder.RegisterModule<TaskDataSqlLiteModule>();

        builder.RegisterAssemblyTypes(ThisAssembly)
            .AsClosedTypesOf(typeof(IDataProvider<>))
            .AsImplementedInterfaces();

        builder.RegisterAssemblyTypes(ThisAssembly)
            .AsClosedTypesOf(typeof(IDataManager<>))
            .AsImplementedInterfaces();
    }
}
