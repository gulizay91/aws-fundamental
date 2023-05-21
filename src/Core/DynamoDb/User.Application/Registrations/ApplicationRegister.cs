using Microsoft.Extensions.DependencyInjection;
using User.Persistence.DynamoDb;

namespace User.Application.Registrations;

public static class ApplicationRegister
{
  public static void RegisterApplication(this IServiceCollection serviceCollection)
  {
    serviceCollection.RegisterMapper();
    serviceCollection.RegisterCqrs();
    //serviceCollection.RegisterDomain(configuration);
    serviceCollection.RegisterRepository();
  }
}