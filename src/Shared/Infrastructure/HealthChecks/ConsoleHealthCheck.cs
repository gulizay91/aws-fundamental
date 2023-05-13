using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Infrastructure.HealthChecks;

public class ConsoleHealthCheck : IHealthCheckPublisher
{
  private readonly string _fileName;
  private HealthStatus _prevStatus = HealthStatus.Unhealthy;

  public ConsoleHealthCheck()
  {
    _fileName = Environment.GetEnvironmentVariable("DOCKER_HEALTHCHECK_FILEPATH") ??
                Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
  }

  /// <summary>
  ///   Creates / touches a file on the file system to indicate "healthy" (liveness) state of the pod
  ///   Deletes the files to indicate "unhealthy"
  ///   The file will then be checked by k8s livenessProbe
  /// </summary>
  /// <param name="report"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  public Task PublishAsync(HealthReport report, CancellationToken cancellationToken)
  {
    var fileExists = _prevStatus == HealthStatus.Healthy;

    if (report.Status == HealthStatus.Healthy)
    {
      using var _ = File.Create(_fileName);
    }
    else if (fileExists)
    {
      File.Delete(_fileName);
    }

    _prevStatus = report.Status;

    return Task.CompletedTask;
  }
}