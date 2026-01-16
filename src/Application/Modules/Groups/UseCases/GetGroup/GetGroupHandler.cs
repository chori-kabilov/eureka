using Application.Abstractions;
using Application.Common;
using Application.Modules.Groups.Dtos;
using Application.Modules.Groups.Mapping;
using Domain.Groups;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Groups.UseCases.GetGroup;

// Получить группу по ID
public class GetGroupHandler(IDataContext db)
{
    public async Task<Result<GroupDetailDto>> HandleAsync(Guid id, CancellationToken ct = default)
    {
        var group = await db.Groups
            .Include(g => g.Course)
            .Include(g => g.ResponsibleTeacher).ThenInclude(t => t.User)
            .Include(g => g.DefaultTeacher).ThenInclude(t => t!.User)
            .Include(g => g.DefaultRoom)
            .Include(g => g.GradingSystem)
            .FirstOrDefaultAsync(g => g.Id == id, ct);

        if (group == null)
            return Result<GroupDetailDto>.Failure(Error.NotFound("Группа"));

        var studentCount = await db.GroupEnrollments
            .CountAsync(e => e.GroupId == id && e.Status == EnrollmentStatus.Active, ct);

        return Result<GroupDetailDto>.Success(GroupMapper.ToDetailDto(group, studentCount));
    }
}
