using Application.Abstractions;
using Application.Common;
using Application.Modules.Schedule.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Schedule.UseCases.ListScheduleTemplates;

// Получить шаблоны расписания группы
public class ListScheduleTemplatesHandler(IDataContext db)
{
    public async Task<Result<List<ScheduleTemplateDto>>> HandleAsync(Guid groupId, CancellationToken ct = default)
    {
        var templates = await db.ScheduleTemplates
            .Include(t => t.Room)
            .Where(t => t.GroupId == groupId)
            .OrderBy(t => t.DayOfWeek)
            .ThenBy(t => t.StartTime)
            .ToListAsync(ct);

        var items = templates.Select(t => new ScheduleTemplateDto
        {
            Id = t.Id,
            GroupId = t.GroupId,
            DayOfWeek = t.DayOfWeek,
            StartTime = t.StartTime,
            EndTime = t.EndTime,
            RoomId = t.RoomId,
            RoomName = t.Room?.Name,
            DefaultLessonType = t.DefaultLessonType,
            IsActive = t.IsActive
        }).ToList();

        return Result<List<ScheduleTemplateDto>>.Success(items);
    }
}
