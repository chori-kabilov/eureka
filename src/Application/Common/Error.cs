namespace Application.Common;

// Модель ошибки
public sealed class Error(string code, string message)
{
    public string Code { get; } = code;
    public string Message { get; } = message;

    // Предопределённые ошибки
    public static readonly Error None = new(string.Empty, string.Empty);
    public static Error NotFound(string entity) => new("NOT_FOUND", $"{entity} не найден");
    public static Error Validation(string message) => new("VALIDATION_ERROR", message);
    public static Error Conflict(string message) => new("CONFLICT", message);
}
