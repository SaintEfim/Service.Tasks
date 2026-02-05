using Autofac;
using FluentValidation;
using Service.Tasks.Domain.Services.Base;
using Service.Tasks.Data.SqlLite;
using Service.Tasks.Domain.Models.Base.Validators;

namespace Service.Tasks.Domain;

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

        builder.RegisterAssemblyTypes(ThisAssembly)
            .AsClosedTypesOf(typeof(IValidator<>));

        builder.RegisterAssemblyTypes(ThisAssembly)
            .AsClosedTypesOf(typeof(IDomainValidator<>));
    }
}
