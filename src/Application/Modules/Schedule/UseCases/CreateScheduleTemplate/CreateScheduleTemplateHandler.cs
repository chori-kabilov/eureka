using Application.Abstractions;
using Application.Common;
using Application.Modules.Schedule.Dtos;
using Domain.Schedule;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Schedule.UseCases.CreateScheduleTemplate;

// Создать шаблон расписания
public class CreateScheduleTemplateHandler(IDataContext db)
{
    public async Task<Result<ScheduleTemplateDto>> HandleAsync(CreateScheduleTemplateRequest request, CancellationToken ct = default)
    {
        var group = await db.Groups.FirstOrDefaultAsync(g => g.Id == request.GroupId, ct);
        if (group == null)
            return Result<ScheduleTemplateDto>.Failure(Error.NotFound("Группа"));

        var startTime = TimeSpan.Parse(request.StartTime);
        var endTime = TimeSpan.Parse(request.EndTime);

        var template = new ScheduleTemplate
        {
            Id = Guid.NewGuid(),
            GroupId = request.GroupId,
            DayOfWeek = (DayOfWeek)request.DayOfWeek,
            StartTime = startTime,
            EndTime = endTime,
            RoomId = request.RoomId,
            DefaultLessonType = request.DefaultLessonType,
            IsActive = true
        };

        db.Add(template);
        await db.SaveChangesAsync(ct);

        return Result<ScheduleTemplateDto>.Success(new ScheduleTemplateDto
        {
            Id = template.Id,
            GroupId = template.GroupId,
            DayOfWeek = template.DayOfWeek,
            StartTime = template.StartTime,
            EndTime = template.EndTime,
            RoomId = template.RoomId,
            DefaultLessonType = template.DefaultLessonType,
            IsActive = template.IsActive
        });
    }
}
