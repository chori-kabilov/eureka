using Application.Abstractions;
using Application.Common;
using Application.Modules.Schedule.Dtos;
using Domain.Enums;
using Domain.Schedule;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Schedule.UseCases;

// Запрос списка занятий
public class ListLessonsRequest
{
    public Guid? GroupId { get; set; }
    public Guid? TeacherId { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public LessonStatus? Status { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}

// Получить список занятий
public class ListLessonsHandler
{
    private readonly IDataContext _db;

    public ListLessonsHandler(IDataContext db) => _db = db;

    public async Task<Result<PagedResult<LessonDto>>> HandleAsync(ListLessonsRequest request, CancellationToken ct = default)
    {
        var query = _db.Lessons
            .Include(l => l.Group)
            .Include(l => l.Teacher).ThenInclude(t => t.User)
            .Include(l => l.Room)
            .AsQueryable();

        if (request.GroupId.HasValue)
            query = query.Where(l => l.GroupId == request.GroupId);

        if (request.TeacherId.HasValue)
            query = query.Where(l => l.TeacherId == request.TeacherId);

        if (request.DateFrom.HasValue)
            query = query.Where(l => l.Date >= request.DateFrom.Value);

        if (request.DateTo.HasValue)
            query = query.Where(l => l.Date <= request.DateTo.Value);

        if (request.Status.HasValue)
            query = query.Where(l => l.Status == request.Status);

        var totalCount = await query.CountAsync(ct);
        var skip = (request.Page - 1) * request.PageSize;

        var lessons = await query
            .OrderBy(l => l.Date)
            .ThenBy(l => l.StartTime)
            .Skip(skip)
            .Take(request.PageSize)
            .ToListAsync(ct);

        var items = lessons.Select(l => new LessonDto
        {
            Id = l.Id,
            GroupId = l.GroupId,
            GroupName = l.Group?.Name ?? string.Empty,
            Date = l.Date,
            StartTime = l.StartTime,
            EndTime = l.EndTime,
            TeacherId = l.TeacherId,
            TeacherName = l.Teacher?.User?.FullName ?? string.Empty,
            RoomId = l.RoomId,
            RoomName = l.Room?.Name,
            Type = l.Type,
            Status = l.Status,
            Topic = l.Topic
        }).ToList();

        return Result<PagedResult<LessonDto>>.Success(new PagedResult<LessonDto>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount
        });
    }
}

// Создать занятие
public class CreateLessonRequest
{
    public Guid GroupId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public Guid TeacherId { get; set; }
    public Guid? RoomId { get; set; }
    public LessonType Type { get; set; } = LessonType.Regular;
    public string? Topic { get; set; }
    public string? Description { get; set; }
}

public class CreateLessonHandler
{
    private readonly IDataContext _db;

    public CreateLessonHandler(IDataContext db) => _db = db;

    public async Task<Result<LessonDto>> HandleAsync(CreateLessonRequest request, CancellationToken ct = default)
    {
        var group = await _db.Groups.FirstOrDefaultAsync(g => g.Id == request.GroupId, ct);
        if (group == null)
            return Result<LessonDto>.Failure(Error.NotFound("Группа"));

        var teacher = await _db.Teachers.Include(t => t.User).FirstOrDefaultAsync(t => t.Id == request.TeacherId, ct);
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

        _db.Add(lesson);
        await _db.SaveChangesAsync(ct);

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

// Генерация занятий из шаблона
public class GenerateLessonsRequest
{
    public Guid GroupId { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}

public class GenerateLessonsHandler
{
    private readonly IDataContext _db;

    public GenerateLessonsHandler(IDataContext db) => _db = db;

    public async Task<Result<int>> HandleAsync(GenerateLessonsRequest request, CancellationToken ct = default)
    {
        var group = await _db.Groups
            .Include(g => g.ScheduleTemplates)
            .FirstOrDefaultAsync(g => g.Id == request.GroupId, ct);

        if (group == null)
            return Result<int>.Failure(Error.NotFound("Группа"));

        var templates = group.ScheduleTemplates.Where(t => t.IsActive).ToList();
        if (!templates.Any())
            return Result<int>.Failure(Error.Validation("Нет активных шаблонов расписания"));

        var teacherId = group.DefaultTeacherId ?? group.ResponsibleTeacherId;
        var generated = 0;

        // Получаем существующие занятия
        var existingDates = await _db.Lessons
            .Where(l => l.GroupId == request.GroupId && 
                       l.Date >= request.FromDate && 
                       l.Date <= request.ToDate)
            .Select(l => new { l.Date, l.StartTime })
            .ToListAsync(ct);

        var existingSet = existingDates.Select(x => $"{x.Date:yyyy-MM-dd}_{x.StartTime}").ToHashSet();

        for (var date = request.FromDate.Date; date <= request.ToDate.Date; date = date.AddDays(1))
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

                _db.Add(lesson);
                generated++;
            }
        }

        await _db.SaveChangesAsync(ct);

        return Result<int>.Success(generated);
    }
}

// Отменить занятие
public class CancelLessonHandler
{
    private readonly IDataContext _db;

    public CancelLessonHandler(IDataContext db) => _db = db;

    public async Task<Result<bool>> HandleAsync(Guid lessonId, string? reason, CancellationToken ct = default)
    {
        var lesson = await _db.Lessons.FirstOrDefaultAsync(l => l.Id == lessonId, ct);
        if (lesson == null)
            return Result<bool>.Failure(Error.NotFound("Занятие"));

        lesson.Status = LessonStatus.Cancelled;
        lesson.CancellationReason = reason;

        await _db.SaveChangesAsync(ct);

        return Result<bool>.Success(true);
    }
}
