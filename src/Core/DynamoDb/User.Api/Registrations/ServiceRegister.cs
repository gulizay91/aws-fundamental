using FluentValidation;
using User.Api.Filters;

namespace User.Api.Registrations;

public static class ServiceRegister
{
  public static void RegisterServices(this IServiceCollection serviceCollection)
  {
    serviceCollection.AddValidatorsFromAssembly(typeof(ValidationFilterAttribute<>).Assembly);
  }
}