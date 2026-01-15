namespace Application.Abstractions;

// Интерфейс для хеширования паролей
public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hash);
}
