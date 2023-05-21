using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using User.Application.Common;

namespace User.Application.Registrations;

public static class CqrsRegister
{
  public static void RegisterCqrs(this IServiceCollection serviceCollection)
  {
    serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    serviceCollection.AddValidatorsFromAssembly(typeof(ValidationBehaviour<,>).Assembly);
  }
}