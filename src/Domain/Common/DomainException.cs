namespace Domain.Common;

// Базовое исключение для доменных ошибок
public class DomainException(string code, string message) : Exception(message)
{
    public string Code { get; } = code;
}
