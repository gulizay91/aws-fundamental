// See https://aka.ms/new-console-template for more information

using Contracts.Commands.V1.User;
using Infrastructure.Constants;
using Infrastructure.Extensions;
using User.Api.LifetimeHooks;
using User.Api.Middlewares;
using User.Api.Registrations;
using User.Api.Services;

Console.WriteLine("Hello, User Api!");

var builder = WebApplication.CreateBuilder(args);
Console.WriteLine("User.Api starting...");
ConfigureHostSettings(builder.Host);
Console.WriteLine("Configured Host Settings...");
ConfigurationSettings(builder.Configuration);
RegisterServices(builder.Services, builder.Configuration);
Console.WriteLine("Services Registered...");


var app = builder.Build();

ConfigureWebApplication(app);


var userRoute = app.MapGroup("/api/v1/users");

userRoute.MapPost("/", async (CreateUser user, IAmazonSqsService amazonSqsService) =>
{
  var response =
    await amazonSqsService.PublishStandartMessageAsync(user, QueueNames.UserQueueName,
      user.GetSqsContractMessageAttributes());
  return Results.Ok(response);
});

userRoute.MapPatch("/", async (UpdateUser user, IAmazonSqsService amazonSqsService) =>
{
  var response =
    await amazonSqsService.PublishStandartMessageAsync(user, QueueNames.UserQueueName,
      user.GetSqsContractMessageAttributes());
  return Results.Ok(response);
});


await app.RunAsync();

void ConfigurationSettings(IConfigurationBuilder configurationBuilder)
{
  configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
  configurationBuilder.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true);
  configurationBuilder.AddEnvironmentVariables();
}

void ConfigureHostSettings(IHostBuilder hostBuilder)
{
  //https://docs.microsoft.com/en-us/aspnet/core/migration/50-to-60-samples?view=aspnetcore-6.0
  // Wait 30 seconds for graceful shutdown.
  hostBuilder.ConfigureHostOptions(o => o.ShutdownTimeout = TimeSpan.FromSeconds(45));
}

void RegisterServices(IServiceCollection serviceCollection, IConfiguration configurationRoot)
{
  serviceCollection.AddHealthChecks();
  serviceCollection.RegisterLoggers(configurationRoot);
  serviceCollection.RegisterSettings(configurationRoot);
  serviceCollection.RegisterSwagger();
  serviceCollection.RegisterServices();

  serviceCollection.AddHostedService<ApplicationLifetimeService>();
}

void ConfigureWebApplication(IApplicationBuilder applicationBuilder)
{
  applicationBuilder.UseHttpsRedirection();
  applicationBuilder
    .UseMiddleware<
      ExceptionHandlerMiddleware>(); // ps: must be the first middleware, to ensure exceptions at all levels are handled
  applicationBuilder.UseRouting();
  applicationBuilder.RegisterHealthCheck();
  applicationBuilder.RegisterRouteHandler();
  applicationBuilder.UseSwagger();
  applicationBuilder.UseSwaggerUI();
}