using Amazon.SQS;
using User.Api.Services;

namespace User.Api.Registrations;

public static class ServiceRegister
{
  public static void RegisterServices(this IServiceCollection serviceCollection)
  {
    serviceCollection.AddSingleton<IAmazonSQS, AmazonSQSClient>(); // register with default values
    //serviceCollection.AddSingleton<Clients.IAmazonClientFactory, Clients.AmazonClientFactory>(); // register with settings
    serviceCollection.AddSingleton<IAmazonSqsService, AmazonSqsService>();
  }
}