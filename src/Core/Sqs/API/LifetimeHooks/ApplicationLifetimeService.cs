using Infrastructure.Loggers;
using Microsoft.Extensions.Hosting;

namespace API.LifetimeHooks;

public class ApplicationLifetimeService : IHostedService
{
  private readonly IHostApplicationLifetime _applicationLifetime;
  private readonly IConsoleLogger _logger;

  public ApplicationLifetimeService(IHostApplicationLifetime applicationLifetime, IConsoleLogger logger)
  {
    _applicationLifetime = applicationLifetime;
    _logger = logger;
  }

  public Task StartAsync(CancellationToken cancellationToken)
  {
    // register a callback that sleeps for 30 seconds
    _applicationLifetime.ApplicationStopping.Register(() =>
    {
      _logger.LogInformation("SIGTERM received, waiting for 30 seconds");
      Thread.Sleep(30_000);
      _logger.LogInformation("Termination delay complete, continuing stopping process");
    });
    return Task.CompletedTask;
  }

  // Required to satisfy interface
  public Task StopAsync(CancellationToken cancellationToken)
  {
    return Task.CompletedTask;
  }
}