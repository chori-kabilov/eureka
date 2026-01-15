namespace Application.Abstractions;

// Интерфейс для получения текущего пользователя
public interface ICurrentUser
{
    Guid? UserId { get; }
    bool IsAuthenticated { get; }
}
