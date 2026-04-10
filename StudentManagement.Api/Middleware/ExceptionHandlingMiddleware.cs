using System.Net;
using System.Text.Json;

namespace StudentManagement.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.Clear();
        context.Response.ContentType = "application/json";

        var response = context.Response;

        var errorResponse = new
        {
            StatusCode = (int)HttpStatusCode.InternalServerError,
            Message = "An internal server error occurred.",
            DetailedMessage = exception.Message,
            Timestamp = DateTime.UtcNow
        };

        switch (exception)
        {
            case ArgumentException argEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse = new
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Invalid request parameters.",
                    DetailedMessage = argEx.Message,
                    Timestamp = DateTime.UtcNow
                };
                break;

            case UnauthorizedAccessException unauthEx:
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                errorResponse = new
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    Message = "Unauthorized access.",
                    DetailedMessage = unauthEx.Message,
                    Timestamp = DateTime.UtcNow
                };
                break;

            case KeyNotFoundException keyEx:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse = new
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Message = "Resource not found.",
                    DetailedMessage = keyEx.Message,
                    Timestamp = DateTime.UtcNow
                };
                break;

            case InvalidOperationException opEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse = new
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Operation not allowed.",
                    DetailedMessage = opEx.Message,
                    Timestamp = DateTime.UtcNow
                };
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }
}
