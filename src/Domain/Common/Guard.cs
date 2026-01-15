namespace Domain.Common;

// Guard clauses для валидации входных данных
public static class Guard
{
    public static void AgainstEmpty(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("EMPTY_VALUE", $"{paramName} не может быть пустым");
    }

    public static void AgainstNull<T>(T value, string paramName) where T : class
    {
        if (value is null)
            throw new DomainException("NULL_VALUE", $"{paramName} не может быть null");
    }

    public static void AgainstNegative(decimal value, string paramName)
    {
        if (value < 0)
            throw new DomainException("NEGATIVE_VALUE", $"{paramName} не может быть отрицательным");
    }

    public static void AgainstZeroOrNegative(int value, string paramName)
    {
        if (value <= 0)
            throw new DomainException("INVALID_VALUE", $"{paramName} должен быть больше нуля");
    }
}
