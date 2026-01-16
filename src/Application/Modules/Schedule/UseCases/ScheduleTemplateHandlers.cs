using Application.Abstractions;
using Application.Common;
using Domain.Enums;
using Domain.Schedule;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Schedule.UseCases;

// DTO шаблона расписания
public class ScheduleTemplateDto
{
    public Guid Id { get; set; }
    public Guid GroupId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public Guid? RoomId { get; set; }
    public string? RoomName { get; set; }
    public LessonType DefaultLessonType { get; set; }
    public bool IsActive { get; set; }
}

// Получить шаблоны расписания группы
public class ListScheduleTemplatesHandler
{
    private readonly IDataContext _db;

    public ListScheduleTemplatesHandler(IDataContext db) => _db = db;

    public async Task<Result<List<ScheduleTemplateDto>>> HandleAsync(Guid groupId, CancellationToken ct = default)
    {
        var templates = await _db.ScheduleTemplates
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

// Создать шаблон расписания
public class CreateScheduleTemplateRequest
{
    public Guid GroupId { get; set; }
    public int DayOfWeek { get; set; }
    public string StartTime { get; set; } = string.Empty;
    public string EndTime { get; set; } = string.Empty;
    public Guid? RoomId { get; set; }
    public LessonType DefaultLessonType { get; set; } = LessonType.Regular;
}

public class CreateScheduleTemplateHandler
{
    private readonly IDataContext _db;

    public CreateScheduleTemplateHandler(IDataContext db) => _db = db;

    public async Task<Result<ScheduleTemplateDto>> HandleAsync(CreateScheduleTemplateRequest request, CancellationToken ct = default)
    {
        var group = await _db.Groups.FirstOrDefaultAsync(g => g.Id == request.GroupId, ct);
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

        _db.Add(template);
        await _db.SaveChangesAsync(ct);

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

// Удалить шаблон
public class DeleteScheduleTemplateHandler
{
    private readonly IDataContext _db;

    public DeleteScheduleTemplateHandler(IDataContext db) => _db = db;

    public async Task<Result<bool>> HandleAsync(Guid id, CancellationToken ct = default)
    {
        var template = await _db.ScheduleTemplates.FirstOrDefaultAsync(t => t.Id == id, ct);
        if (template == null)
            return Result<bool>.Failure(Error.NotFound("Шаблон"));

        _db.Remove(template);
        await _db.SaveChangesAsync(ct);

        return Result<bool>.Success(true);
    }
}
