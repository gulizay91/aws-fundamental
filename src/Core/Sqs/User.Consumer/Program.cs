using System.Reflection;
using Amazon.SQS;
using Infrastructure.Extensions;
using Infrastructure.Loggers;
using User.Consumer.Services;
using User.Consumer.Services.AmazonSqs;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine("User.Consumer starting...");
ConfigureHostSettings(builder.Host);
Console.WriteLine("Configured Host Settings...");
ConfigurationSettings(builder.Configuration);
RegisterServices(builder.Services, builder.Configuration);
Console.WriteLine("Services Registered...");

var app = builder.Build();

//app.MapGet("/", () => "Hello World!");

app.Run();


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

  serviceCollection.AddSingleton<ILoggerFactory, LoggerFactory>();
  serviceCollection.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
  var defaultLogLevel = configurationRoot.GetSection("Logging:Console:LogLevel:Default").Value.ToEnum(LogLevel.Error);
  Console.Out.WriteLine($"Console:LogLevel:Default: {defaultLogLevel}");
  ConsoleLogger.DefaultLogLevel = defaultLogLevel;
  serviceCollection.AddSingleton<IConsoleLogger, ConsoleLogger>();

  serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

  serviceCollection.AddSingleton<IAmazonSQS, AmazonSQSClient>(); // register with default values
  serviceCollection.AddSingleton<IAmazonSqsService, AmazonSqsService>();
  //GC.KeepAlive(Assembly.Load(Assembly.GetAssembly(typeof(IContract)).GetName()));
  serviceCollection.AddHostedService<ConsumerWorkerService>();
}