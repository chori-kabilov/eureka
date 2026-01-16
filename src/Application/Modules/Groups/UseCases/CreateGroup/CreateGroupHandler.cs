using Application.Abstractions;
using Application.Common;
using Application.Modules.Groups.Dtos;
using Application.Modules.Groups.Mapping;
using Domain.Groups;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Groups.UseCases.CreateGroup;

// Создать группу
public class CreateGroupHandler(IDataContext db)
{
    public async Task<Result<GroupDetailDto>> HandleAsync(CreateGroupRequest request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return Result<GroupDetailDto>.Failure(Error.Validation("Название группы обязательно"));

        var course = await db.Courses.FirstOrDefaultAsync(c => c.Id == request.CourseId, ct);
        if (course == null)
            return Result<GroupDetailDto>.Failure(Error.NotFound("Курс"));

        var teacher = await db.Teachers.Include(t => t.User).FirstOrDefaultAsync(t => t.Id == request.ResponsibleTeacherId, ct);
        if (teacher == null)
            return Result<GroupDetailDto>.Failure(Error.NotFound("Учитель"));

        var group = new Group
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Code = request.Code?.Trim(),
            CourseId = request.CourseId,
            ResponsibleTeacherId = request.ResponsibleTeacherId,
            DefaultTeacherId = request.DefaultTeacherId ?? request.ResponsibleTeacherId,
            DefaultRoomId = request.DefaultRoomId,
            GradingSystemId = request.GradingSystemId,
            StartDate = DateTime.SpecifyKind(request.StartDate, DateTimeKind.Utc),
            EndDate = request.EndDate.HasValue ? DateTime.SpecifyKind(request.EndDate.Value, DateTimeKind.Utc) : null,
            MaxStudents = request.MaxStudents,
            Status = GroupStatus.Draft,
            Notes = request.Notes
        };

        db.Add(group);
        await db.SaveChangesAsync(ct);

        group.Course = course;
        group.ResponsibleTeacher = teacher;

        return Result<GroupDetailDto>.Success(GroupMapper.ToDetailDto(group, 0));
    }
}
