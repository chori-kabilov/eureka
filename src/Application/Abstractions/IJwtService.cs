using Domain.Users;

namespace Application.Abstractions;

// Интерфейс для генерации JWT токенов
public interface IJwtService
{
    string GenerateToken(User user);
}
