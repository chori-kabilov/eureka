using Application.Abstractions;
using Application.Common;
using Application.Modules.Auth.Dtos;
using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Auth.UseCases.Login;

// Handler входа пользователя
public class LoginHandler
{
    private readonly IDataContext _db;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    public LoginHandler(
        IDataContext db,
        IPasswordHasher passwordHasher,
        IJwtService jwtService)
    {
        _db = db;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public async Task<Result<AuthResultDto>> HandleAsync(
        LoginRequest request,
        CancellationToken ct = default)
    {
        // Валидация
        Guard.AgainstEmpty(request.Phone, "Телефон");
        Guard.AgainstEmpty(request.Password, "Пароль");

        // Поиск пользователя
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Phone == request.Phone, ct);

        if (user == null)
            return Result<AuthResultDto>.Failure(
                Error.NotFound("Пользователь не найден"));

        // Проверка пароля
        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
            return Result<AuthResultDto>.Failure(
                new Error("INVALID_PASSWORD", "Неверный пароль"));

        // Генерация токена
        var token = _jwtService.GenerateToken(user);

        return Result<AuthResultDto>.Success(new AuthResultDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            Phone = user.Phone,
            Role = user.Role,
            Token = token
        });
    }
}
