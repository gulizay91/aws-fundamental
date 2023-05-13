using Infrastructure.Constants;
using Infrastructure.Extensions;
using Infrastructure.ObjectMothers.User;
using Microsoft.Extensions.Hosting;
using SqsPublisher.Services.AmazonServices;

namespace SqsPublisher;

public class Worker : BackgroundService
{
  private readonly IAmazonSqsService _amazonSqsService;
  private string? _key;

  public Worker(IAmazonSqsService amazonSqsService)
  {
    _amazonSqsService = amazonSqsService;
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    await Task.Delay(3000, stoppingToken);
    while (!stoppingToken.IsCancellationRequested && _key != "q" && _key != "Q") await Menu(stoppingToken);
    await Task.Delay(1000, stoppingToken);
    Environment.Exit(0);
  }

  private async Task Menu(CancellationToken stoppingToken)
  {
    Console.WriteLine("Sqs Publisher Client Main Menu");
    Console.WriteLine("***************************");
    Console.WriteLine("Select Process or Exit(q || Q)");
    Console.WriteLine("1- SQS CreateUser Command");
    Console.WriteLine("***************************");
    Console.Write("Enter your choice: ");
    _key = Console.ReadLine();
    switch (_key)
    {
      case "1":
        var commandCreateUser = ContractObjectMother.SimpleFakeCreateUser();
        await _amazonSqsService.PublishStandartMessageAsync(commandCreateUser, QueueNames.UserQueueName,
          commandCreateUser.GetCommandMessageInfo());
        break;
      case "q":
      case "Q":
        Console.WriteLine("Goodbye!");
        break;
      default:
        Console.WriteLine("Invalid Key");
        break;
    }
  }
}