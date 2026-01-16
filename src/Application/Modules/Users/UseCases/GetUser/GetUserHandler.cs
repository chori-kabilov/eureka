using Application.Abstractions;
using Application.Common;
using Application.Modules.Users.Dtos;
using Application.Modules.Users.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Users.UseCases.GetUser;

// Handler получения пользователя по ID
public class GetUserHandler(IDataContext db)
{
    public async Task<Result<UserDetailDto>> HandleAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await db.Users
            .Include(u => u.AdminProfile)
            .Include(u => u.StudentProfile)
            .Include(u => u.TeacherProfile)
            .Include(u => u.ParentProfile)
            .FirstOrDefaultAsync(u => u.Id == userId, ct);

        if (user == null)
            return Result<UserDetailDto>.Failure(Error.NotFound("Пользователь"));

        return Result<UserDetailDto>.Success(UserMapper.ToDetailDto(user));
    }
}
