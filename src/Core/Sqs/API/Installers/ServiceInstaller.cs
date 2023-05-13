using Amazon.SQS;
using API.Services;
using Microsoft.Extensions.DependencyInjection;

namespace API.Installers;

public static class ServiceInstaller
{
  public static void InstallServices(this IServiceCollection serviceCollection)
  {
    serviceCollection.AddScoped<IAmazonSQS, AmazonSQSClient>(); // register with default values
    //serviceCollection.AddScoped<Clients.IAmazonClientFactory, Clients.AmazonClientFactory>(); // register with settings
    serviceCollection.AddScoped<IAmazonSqsService, AmazonSqsService>();
  }
}