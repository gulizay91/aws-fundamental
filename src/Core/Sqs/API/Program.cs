// See https://aka.ms/new-console-template for more information

using API.Installers;
using API.LifetimeHooks;
using API.Middlewares;
using API.Services;
using Contracts.Commands.V1.User;
using Infrastructure.Constants;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Hello, Api!");

var builder = WebApplication.CreateBuilder(args);
Console.WriteLine("API starting...");
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
    await amazonSqsService.PublishStandartMessageAsync(user, QueueNames.UserQueueName, user.GetCommandMessageInfo());
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
  serviceCollection.InstallLoggers(configurationRoot);
  serviceCollection.InstallSettings(configurationRoot);
  serviceCollection.InstallSwagger();
  serviceCollection.InstallServices();
  
  serviceCollection.AddHostedService<ApplicationLifetimeService>();
}

void ConfigureWebApplication(IApplicationBuilder applicationBuilder)
{
  applicationBuilder.UseHttpsRedirection();
  applicationBuilder
    .UseMiddleware<
      ExceptionHandlerMiddleware>(); // ps: must be the first middleware, to ensure exceptions at all levels are handled
  applicationBuilder.UseRouting();
  applicationBuilder.InstallHealthCheck();
  applicationBuilder.InstallRouteHandler();
  applicationBuilder.UseSwagger();
  applicationBuilder.UseSwaggerUI();
}