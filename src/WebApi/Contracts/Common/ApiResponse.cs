namespace WebApi.Contracts.Common;

// Унифицированный ответ API
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public ErrorResponse? Error { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public static ApiResponse<T> Ok(T data) => new()
    {
        Success = true,
        Data = data
    };

    public static ApiResponse<T> Fail(string code, string message) => new()
    {
        Success = false,
        Error = new ErrorResponse { Code = code, Message = message }
    };
}
