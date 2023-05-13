using Infrastructure.Extensions;
using Infrastructure.Loggers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace API.Installers;

public static class LoggerInstaller
{
  public static void InstallLoggers(this IServiceCollection serviceCollection, IConfiguration configuration)
  {
    //serviceCollection.AddSingleton<ILoggerFactory, LoggerFactory>();
    //serviceCollection.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
    var defaultLogLevel = configuration.GetSection("Logging:Console:LogLevel:Default").Value.ToEnum(LogLevel.Error);
    Console.Out.WriteLine($"Console:LogLevel:Default: {defaultLogLevel}");
    ConsoleLogger.DefaultLogLevel = defaultLogLevel;
    serviceCollection.AddSingleton<IConsoleLogger, ConsoleLogger>();
  }
}