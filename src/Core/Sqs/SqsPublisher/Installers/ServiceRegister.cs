using Amazon.SQS;
using Microsoft.Extensions.DependencyInjection;
using SqsPublisher.Services.AmazonServices;

namespace SqsPublisher.Installers;

public static class ServiceRegister
{
  public static void RegisterServices(this IServiceCollection serviceCollection)
  {
    serviceCollection.AddScoped<IAmazonSQS, AmazonSQSClient>(); // register with default values
    //serviceCollection.AddScoped<Clients.IAmazonClientFactory, Clients.AmazonClientFactory>(); // register with settings
    serviceCollection.AddScoped<IAmazonSqsService, AmazonSqsService>();

    serviceCollection.AddHostedService<Worker>();
  }
}