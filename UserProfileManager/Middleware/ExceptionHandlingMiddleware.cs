using System.Net;
using UserProfileManager.DTOs;
using Microsoft.Extensions.Logging;

namespace UserProfileManager.Middleware
{
  public class ExceptionHandlingMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
      _next = next;
      _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
      try
      {
        await _next(httpContext);
      }
      catch (Exception ex)
      {
        _logger.LogError($"Exception occurred: {ex.Message}");
        await HandleExceptionAsync(httpContext, ex);
      }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
      context.Response.ContentType = "application/json";

      var (statusCode, message) = GetExceptionDetails(exception);

      context.Response.StatusCode = statusCode;

      ErrorDetails errorDetails = new ()
      {
        StatusCode = statusCode,
        Message = message,
        Detail = exception.StackTrace ?? string.Empty // Optionally include stack trace for debugging
      };

      // Optionally, you can log the error details
      _logger.LogError($"Error Details: {errorDetails}");

      return context.Response.WriteAsync(errorDetails.ToString());
    }

    private (int statusCode, string message) GetExceptionDetails(Exception exception)
    {
      return exception switch
      {
        ArgumentNullException _ => ((int)HttpStatusCode.BadRequest, "A required argument was null."),
        UnauthorizedAccessException _ => ((int)HttpStatusCode.Unauthorized, "Unauthorized access."),
        KeyNotFoundException _ => ((int)HttpStatusCode.NotFound, "Resource not found."),
        InvalidOperationException _ => ((int)HttpStatusCode.Conflict, "Invalid operation."),
        _ => ((int)HttpStatusCode.InternalServerError, "Internal Server Error from the custom middleware.")
      };
    }
  }
}
