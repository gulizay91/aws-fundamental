using Amazon.SimpleNotificationService;
using User.Api.Services;

namespace User.Api.Registrations;

public static class ServiceRegister
{
  public static void RegisterServices(this IServiceCollection serviceCollection)
  {
    serviceCollection
      .AddSingleton<IAmazonSimpleNotificationService,
        AmazonSimpleNotificationServiceClient>(); // register with default values
    serviceCollection.AddSingleton<IAmazonSnsService, AmazonSnsService>();
  }
}