using Application.Abstractions;
using Application.Common;
using Application.Modules.Schedule.Dtos;
using Domain.Schedule;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Schedule.UseCases.CreateLesson;

// Создать занятие
public class CreateLessonHandler(IDataContext db)
{
    public async Task<Result<LessonDto>> HandleAsync(CreateLessonRequest request, CancellationToken ct = default)
    {
        var group = await db.Groups.FirstOrDefaultAsync(g => g.Id == request.GroupId, ct);
        if (group == null)
            return Result<LessonDto>.Failure(Error.NotFound("Группа"));

        var teacher = await db.Teachers.Include(t => t.User).FirstOrDefaultAsync(t => t.Id == request.TeacherId, ct);
        if (teacher == null)
            return Result<LessonDto>.Failure(Error.NotFound("Учитель"));

        var lesson = new Lesson
        {
            Id = Guid.NewGuid(),
            GroupId = request.GroupId,
            Date = DateTime.SpecifyKind(request.Date.Date, DateTimeKind.Utc),
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            TeacherId = request.TeacherId,
            RoomId = request.RoomId,
            Type = request.Type,
            Status = LessonStatus.Planned,
            Topic = request.Topic,
            Description = request.Description
        };

        db.Add(lesson);
        await db.SaveChangesAsync(ct);

        return Result<LessonDto>.Success(new LessonDto
        {
            Id = lesson.Id,
            GroupId = lesson.GroupId,
            GroupName = group.Name,
            Date = lesson.Date,
            StartTime = lesson.StartTime,
            EndTime = lesson.EndTime,
            TeacherId = lesson.TeacherId,
            TeacherName = teacher.User?.FullName ?? string.Empty,
            RoomId = lesson.RoomId,
            Type = lesson.Type,
            Status = lesson.Status,
            Topic = lesson.Topic
        });
    }
}
