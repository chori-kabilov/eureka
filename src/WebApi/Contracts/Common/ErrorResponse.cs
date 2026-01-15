namespace WebApi.Contracts.Common;

// Формат ошибки в ответе API
public class ErrorResponse
{
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
