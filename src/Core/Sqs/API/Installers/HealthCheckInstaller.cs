using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace API.Installers;

public static class HealthCheckInstaller
{
  public static void InstallHealthCheck(this IApplicationBuilder applicationBuilder)
  {
    //for liveness probe
    applicationBuilder.UseEndpoints(endpoints =>
    {
      endpoints.MapHealthChecks("/health", new HealthCheckOptions
      {
        Predicate = _ => false
      });
    });

    //for readiness probe - mongodb & masstransit. 
    //note: masstransit's health check tags - masstransit, ready.
    applicationBuilder.UseEndpoints(endpoints =>
    {
      endpoints.MapHealthChecks("/ready", new HealthCheckOptions
      {
        Predicate = check => check.Tags.Contains("ready")
      });
    });
  }
}