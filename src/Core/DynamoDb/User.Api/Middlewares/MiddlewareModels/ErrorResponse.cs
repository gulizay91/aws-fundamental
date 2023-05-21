namespace User.Api.Middlewares.MiddlewareModels;

public record ErrorResponse
{
  public string Message { get; init; }
}