using Application.Abstractions;
using Application.Common;
using Domain.Schedule;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Schedule.UseCases.GenerateLessons;

// Генерация занятий из шаблона
public class GenerateLessonsHandler(IDataContext db)
{
    public async Task<Result<int>> HandleAsync(GenerateLessonsRequest request, CancellationToken ct = default)
    {
        var group = await db.Groups
            .Include(g => g.ScheduleTemplates)
            .FirstOrDefaultAsync(g => g.Id == request.GroupId, ct);

        if (group == null)
            return Result<int>.Failure(Error.NotFound("Группа"));

        var templates = group.ScheduleTemplates.Where(t => t.IsActive).ToList();
        if (!templates.Any())
            return Result<int>.Failure(Error.Validation("Нет активных шаблонов расписания"));

        var teacherId = group.DefaultTeacherId ?? group.ResponsibleTeacherId;
        var generated = 0;

        // Конвертируем даты в UTC для PostgreSQL
        var fromDateUtc = DateTime.SpecifyKind(request.FromDate.Date, DateTimeKind.Utc);
        var toDateUtc = DateTime.SpecifyKind(request.ToDate.Date, DateTimeKind.Utc);

        var existingDates = await db.Lessons
            .Where(l => l.GroupId == request.GroupId && 
                       l.Date >= fromDateUtc && 
                       l.Date <= toDateUtc)
            .Select(l => new { l.Date, l.StartTime })
            .ToListAsync(ct);

        var existingSet = existingDates.Select(x => $"{x.Date:yyyy-MM-dd}_{x.StartTime}").ToHashSet();

        for (var date = fromDateUtc; date <= toDateUtc; date = date.AddDays(1))
        {
            foreach (var template in templates.Where(t => t.DayOfWeek == date.DayOfWeek))
            {
                var key = $"{date:yyyy-MM-dd}_{template.StartTime}";
                if (existingSet.Contains(key))
                    continue;

                var lesson = new Lesson
                {
                    Id = Guid.NewGuid(),
                    GroupId = group.Id,
                    ScheduleTemplateId = template.Id,
                    Date = DateTime.SpecifyKind(date, DateTimeKind.Utc),
                    StartTime = template.StartTime,
                    EndTime = template.EndTime,
                    TeacherId = teacherId,
                    RoomId = template.RoomId ?? group.DefaultRoomId,
                    Type = template.DefaultLessonType,
                    Status = LessonStatus.Planned
                };

                db.Add(lesson);
                generated++;
            }
        }

        await db.SaveChangesAsync(ct);

        return Result<int>.Success(generated);
    }
}
