using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Service.Tasks.API.Extensions;
using Service.Tasks.API.Handlers;
using Service.Tasks.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services
    .AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
    });

builder.Services.AddExceptionHandler<ExceptionHandlerBase>();
builder.Services.AddProblemDetails();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly, Assembly.GetExecutingAssembly());

builder.Services.AddMySwagger();
builder.Services.AddMyAuthentication(builder.Configuration);

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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();
