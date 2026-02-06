using Autofac;
using FluentValidation;
using Service.Tasks.Domain.Services.Base;
using Service.Tasks.Data.SqlLite;
using Service.Tasks.Domain.Models.Base.Validators;
using Service.Tasks.Domain.Services.User;
using Service.Tasks.UserHelpers;
using Service.Tasks.UserJWTToken;
using Service.Tasks.UserJWTToken.Helpers;

namespace Service.Tasks.Domain;

public class TaskDomainModule : Module
{
    protected override void Load(
        ContainerBuilder builder)
    {
        builder.RegisterModule<TaskDataSqlLiteModule>();
        builder.RegisterModule<UserPasswordHasherModule>();
        builder.RegisterModule<UserJwtTokenModule>();

        builder.RegisterType<UserManager>()
            .As<IUserManager>();

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
