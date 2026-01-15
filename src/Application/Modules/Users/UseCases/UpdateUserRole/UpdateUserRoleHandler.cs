using Application.Abstractions;
using Application.Common;
using Application.Modules.Users.Dtos;
using Application.Modules.Users.Mapping;
using Domain.Admins;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Users.UseCases.UpdateUserRole;

// Handler изменения Admin статуса пользователя
public class UpdateUserRoleHandler
{
    private readonly IDataContext _db;
    private readonly ICurrentUser _currentUser;

    public UpdateUserRoleHandler(IDataContext db, ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<Result<UserDetailDto>> HandleAsync(
        UpdateUserRoleRequest request,
        CancellationToken ct = default)
    {
        // Найти пользователя с профилями
        var user = await _db.Users
            .Include(u => u.AdminProfile)
            .Include(u => u.StudentProfile)
            .Include(u => u.TeacherProfile)
            .Include(u => u.ParentProfile)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, ct);

        if (user == null)
            return Result<UserDetailDto>.Failure(Error.NotFound("Пользователь"));

        // Защита: нельзя менять себя
        if (_currentUser.UserId == user.Id)
            return Result<UserDetailDto>.Failure(
                new Error("SELF_ROLE_CHANGE", "Нельзя изменить свою роль"));

        // Изменить Admin статус
        if (request.IsAdmin && user.AdminProfile == null)
        {
            // Создать Admin профиль
            var adminProfile = new Admin
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                AccessLevel = AdminAccessLevel.Limited
            };
            _db.Add(adminProfile);
        }
        else if (!request.IsAdmin && user.AdminProfile != null)
        {
            // Удалить Admin профиль
            _db.Remove(user.AdminProfile);
        }

        await _db.SaveChangesAsync(ct);

        // Перезагрузить пользователя
        var updatedUser = await _db.Users
            .Include(u => u.AdminProfile)
            .Include(u => u.StudentProfile)
            .Include(u => u.TeacherProfile)
            .Include(u => u.ParentProfile)
            .FirstAsync(u => u.Id == request.UserId, ct);

        return Result<UserDetailDto>.Success(UserMapper.ToDetailDto(updatedUser));
    }
}
