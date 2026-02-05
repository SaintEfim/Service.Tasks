using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Service.Tasks.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services
    .AddControllers()
    .AddNewtonsoftJson();

builder.Services.AddOpenApiDocument();

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
app.MapControllers();

await app.RunAsync();
