using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Files.Api.Configurations;

public static class HealthCheckRegister
{
  public static void RegisterHealthCheck(this IApplicationBuilder applicationBuilder)
  {
    //for liveness probe
    applicationBuilder.UseEndpoints(endpoints =>
    {
      endpoints.MapHealthChecks("/health", new HealthCheckOptions
      {
        Predicate = _ => false
      });
    });
  }
}