using Infrastructure.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace SqsPublisher.Installers;

public static class HealthCheckRegister
{
  public static void RegisterHealthCheck(this IServiceCollection serviceCollection)
  {
    serviceCollection.AddSingleton<IHealthCheckPublisher, ConsoleHealthCheck>();
    serviceCollection.Configure<HealthCheckPublisherOptions>(options =>
    {
      options.Delay = TimeSpan.FromSeconds(5);
      options.Period = TimeSpan.FromSeconds(20);
    });
  }
}