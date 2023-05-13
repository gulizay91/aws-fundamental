using Microsoft.Extensions.DependencyInjection;

namespace API.Installers;

public static class SwaggerInstaller
{
  public static void InstallSwagger(this IServiceCollection serviceCollection)
  {
    serviceCollection.AddEndpointsApiExplorer();
    serviceCollection.AddSwaggerGen();
  }
}