using MediatR;
using User.Api.Filters;
using User.Api.LifetimeHooks;
using User.Api.Middlewares;
using User.Api.Registrations;
using User.Application.Commands.User;
using User.Application.Queries.User;
using User.Application.Registrations;

Console.WriteLine("Hello, DynamoDb User Api! CRUD on dynamodb");

var builder = WebApplication.CreateBuilder(args);
Console.WriteLine("User.Api starting...");
ConfigureHostSettings(builder.Host);
Console.WriteLine("Configured Host Settings...");
ConfigurationSettings(builder.Configuration);
RegisterServices(builder.Services, builder.Configuration);
Console.WriteLine("Services Registered...");


var app = builder.Build();

ConfigureWebApplication(app);

ConfigureMinimalApiRoute();


app.Run();


#region BuilderConfigure

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
  serviceCollection.RegisterSwagger();
  serviceCollection.RegisterServices();
  serviceCollection.RegisterApplication();
  
  serviceCollection.AddHostedService<ApplicationLifetimeService>();
}

#endregion

#region ApplicationConfigure

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


void ConfigureMinimalApiRoute()
{

  var userRoute = app.MapGroup("/api/v1/users");

  userRoute.MapPost("/", async (CreateUser user, IMediator mediator) =>
  {
    var response = await mediator.Send(user);
    return response ? Results.Ok(response) : Results.Problem();
  }).AddEndpointFilter<ValidationFilterAttribute<CreateUser>>();
  
  userRoute.MapGet("/{userName}", async (string userName, IMediator mediator) =>
  {
    ArgumentException.ThrowIfNullOrEmpty(userName);
    var response = await mediator.Send(new GetUserByUserName { UserName = userName });
    return response is not null ? Results.Ok(response) : Results.BadRequest(response);
  });

  // userRoute.MapPatch("/", async (UpdateUser user, IMediator _mediator) =>
  // {
  //   var response =
  //     await amazonSqsService.PublishStandartMessageAsync(user, QueueNames.UserQueueName,
  //       user.GetSnsContractMessageAttributes());
  //   return Results.Ok(response);
  // });
}

#endregion