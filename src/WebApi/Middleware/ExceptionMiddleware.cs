using System.Net;
using System.Text.Json;
using Domain.Common;
using WebApi.Contracts.Common;

namespace WebApi.Middleware;

// Middleware для глобальной обработки исключений
public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, code, message) = exception switch
        {
            DomainException domainEx => (HttpStatusCode.BadRequest, domainEx.Code, domainEx.Message),
            KeyNotFoundException => (HttpStatusCode.NotFound, "NOT_FOUND", "Ресурс не найден"),
            _ => (HttpStatusCode.InternalServerError, "INTERNAL_ERROR", "Внутренняя ошибка сервера")
        };

        logger.LogError(exception, "Ошибка: {Code} - {Message}", code, message);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = ApiResponse<object>.Fail(code, message);
        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}
