using Microsoft.AspNetCore.Builder;

namespace API.Installers;

public static class RouteHandlerInstaller
{
  public static void InstallRouteHandler(this IApplicationBuilder applicationBuilder)
  {
    //applicationBuilder.MapGet("/", () => "Hello World!");
  }
}