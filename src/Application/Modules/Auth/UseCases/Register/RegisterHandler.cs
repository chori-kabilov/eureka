using Application.Abstractions;
using Application.Common;
using Application.Modules.Auth.Dtos;
using Domain.Common;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Auth.UseCases.Register;

// Handler регистрации пользователя
public class RegisterHandler(
    IDataContext db,
    IPasswordHasher passwordHasher,
    IJwtService jwtService)
{
    public async Task<Result<AuthResultDto>> HandleAsync(
        RegisterRequest request,
        CancellationToken ct = default)
    {
        // Валидация
        Guard.AgainstEmpty(request.Phone, "Телефон");
        Guard.AgainstEmpty(request.FullName, "ФИО");
        Guard.AgainstEmpty(request.Password, "Пароль");

        // Проверка уникальности телефона
        var existingUser = await db.Users
            .FirstOrDefaultAsync(u => u.Phone == request.Phone, ct);

        if (existingUser != null)
            return Result<AuthResultDto>.Failure(
                Error.Conflict("Пользователь с таким номером телефона уже существует"));

        // Создание пользователя
        var user = new User
        {
            Id = Guid.NewGuid(),
            Phone = request.Phone.Trim(),
            FullName = request.FullName.Trim(),
            PasswordHash = passwordHasher.Hash(request.Password)
        };

        db.Add(user);
        await db.SaveChangesAsync(ct);

        // Генерация токена
        var token = jwtService.GenerateToken(user);

        return Result<AuthResultDto>.Success(new AuthResultDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            Phone = user.Phone,
            IsAdmin = false,
            IsStudent = false,
            IsTeacher = false,
            IsParent = false,
            Token = token
        });
    }
}
