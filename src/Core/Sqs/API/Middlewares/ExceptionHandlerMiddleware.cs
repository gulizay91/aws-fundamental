using System.Net;
using System.Net.Mime;
using System.Text.Json;
using API.Middlewares.MiddlewareModels;
using Infrastructure.Loggers;
using Microsoft.AspNetCore.Http;

namespace API.Middlewares;

public class ExceptionHandlerMiddleware
{
  private readonly IConsoleLogger _consoleLogger;
  private readonly RequestDelegate _next;

  public ExceptionHandlerMiddleware(RequestDelegate next, IConsoleLogger consoleLogger)
  {
    _consoleLogger = consoleLogger;
    _next = next;
  }

  public async Task InvokeAsync(HttpContext httpContext)
  {
    try
    {
      await _next(httpContext);
    }
    catch (Exception ex)
    {
      var message = string.IsNullOrWhiteSpace(ex.Message)
        ? "Internal Server Error from the custom middleware."
        : ex.Message;
      await _consoleLogger.LogError(message, ex, null, null, new HttpMethod(httpContext.Request.Method),
        (HttpStatusCode)httpContext.Response.StatusCode, null, httpContext.Request.Host.Value,
        httpContext.Request.Path);
      await HandleExceptionAsync(httpContext, ex);
    }
  }

  private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
  {
    var message = string.IsNullOrWhiteSpace(exception.Message)
      ? "Internal Server Error from the custom middleware."
      : exception.Message;
    httpContext.Response.ContentType = MediaTypeNames.Application.Json;
    httpContext.Response.StatusCode = exception switch
    {
      //UnauthorizedException => (int)HttpStatusCode.Unauthorized,
      _ => (int)HttpStatusCode.InternalServerError
    };

    var result = JsonSerializer.Serialize(new ErrorResponse { Message = message });
    await httpContext.Response.WriteAsync(result);
  }
}