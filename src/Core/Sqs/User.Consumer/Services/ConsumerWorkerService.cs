using Infrastructure.Constants;
using User.Consumer.Services.AmazonSqs;

namespace User.Consumer.Services;

public class ConsumerWorkerService : BackgroundService
{
  private readonly IAmazonSqsService _amazonSqsService;
  private readonly ILogger<ConsumerWorkerService> _logger;

  public ConsumerWorkerService(ILogger<ConsumerWorkerService> logger, IServiceProvider serviceProvider)
  {
    // Service worker are singleton and do not support scoped transactions. So inject a scoped service into a singleton service 
    _amazonSqsService = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IAmazonSqsService>();
    _logger = logger;
  }

  protected override async Task ExecuteAsync(CancellationToken cancellationToken)
  {
    _logger.LogInformation("Start ConsumerWorkerService...");
    while (!cancellationToken.IsCancellationRequested) await UserConsumer(cancellationToken);
    _logger.LogInformation("End ConsumerWorkerService...");
    await Task.Delay(1000, cancellationToken);
  }

  private async Task UserConsumer(CancellationToken cancellationToken)
  {
    _logger.LogDebug("UserConsumer start consume");
    await _amazonSqsService.SubscribeStandartMessageAsync(cancellationToken, QueueNames.UserQueueName);
    _logger.LogDebug("UserConsumer end consume");
    await Task.Delay(1000, cancellationToken);
  }
}