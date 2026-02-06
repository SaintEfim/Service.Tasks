using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Service.Tasks.API.Handlers;
using Service.Tasks.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services
    .AddControllers()
    .AddNewtonsoftJson();

builder.Services.AddOpenApiDocument();

builder.Services.AddExceptionHandler<ExceptionHandlerBase>();
builder.Services.AddProblemDetails();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly, Assembly.GetExecutingAssembly());

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule<TaskDomainModule>();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.UseRouting();
app.UseExceptionHandler();
app.MapControllers();

await app.RunAsync();
