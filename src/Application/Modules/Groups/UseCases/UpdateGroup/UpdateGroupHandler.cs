using Application.Abstractions;
using Application.Common;
using Application.Modules.Groups.Dtos;
using Application.Modules.Groups.Mapping;
using Domain.Groups;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Groups.UseCases.UpdateGroup;

// Обновить группу
public class UpdateGroupHandler(IDataContext db)
{
    public async Task<Result<GroupDetailDto>> HandleAsync(UpdateGroupRequest request, CancellationToken ct = default)
    {
        var group = await db.Groups
            .Include(g => g.Course)
            .Include(g => g.ResponsibleTeacher).ThenInclude(t => t.User)
            .FirstOrDefaultAsync(g => g.Id == request.Id, ct);

        if (group == null)
            return Result<GroupDetailDto>.Failure(Error.NotFound("Группа"));

        group.Name = request.Name.Trim();
        group.Code = request.Code?.Trim();
        group.ResponsibleTeacherId = request.ResponsibleTeacherId;
        group.DefaultTeacherId = request.DefaultTeacherId;
        group.DefaultRoomId = request.DefaultRoomId;
        group.GradingSystemId = request.GradingSystemId;
        group.StartDate = DateTime.SpecifyKind(request.StartDate, DateTimeKind.Utc);
        group.EndDate = request.EndDate.HasValue ? DateTime.SpecifyKind(request.EndDate.Value, DateTimeKind.Utc) : null;
        group.MaxStudents = request.MaxStudents;
        group.Status = request.Status;
        group.Notes = request.Notes;

        await db.SaveChangesAsync(ct);

        var studentCount = await db.GroupEnrollments
            .CountAsync(e => e.GroupId == group.Id && e.Status == EnrollmentStatus.Active, ct);

        return Result<GroupDetailDto>.Success(GroupMapper.ToDetailDto(group, studentCount));
    }
}
