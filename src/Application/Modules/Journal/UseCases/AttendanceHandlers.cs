using Application.Abstractions;
using Application.Common;
using Application.Modules.Journal.Dtos;
using Domain.Enums;
using Domain.Journal;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Journal.UseCases;

// Получить посещаемость занятия
public class GetLessonAttendanceHandler
{
    private readonly IDataContext _db;

    public GetLessonAttendanceHandler(IDataContext db) => _db = db;

    public async Task<Result<List<AttendanceDto>>> HandleAsync(Guid lessonId, CancellationToken ct = default)
    {
        var attendances = await _db.Attendances
            .Include(a => a.Student).ThenInclude(s => s!.User)
            .Include(a => a.Child)
            .Include(a => a.MarkedBy)
            .Where(a => a.LessonId == lessonId)
            .ToListAsync(ct);

        var items = attendances.Select(a => new AttendanceDto
        {
            Id = a.Id,
            LessonId = a.LessonId,
            StudentId = a.StudentId,
            ChildId = a.ChildId,
            StudentName = a.Student?.User?.FullName ?? a.Child?.FullName ?? string.Empty,
            Status = a.Status,
            ArrivalTime = a.ArrivalTime,
            LeaveTime = a.LeaveTime,
            ExcuseReason = a.ExcuseReason,
            MarkedAt = a.MarkedAt,
            MarkedByName = a.MarkedBy?.FullName ?? string.Empty
        }).ToList();

        return Result<List<AttendanceDto>>.Success(items);
    }
}

// Отметить посещаемость
public class MarkAttendanceRequest
{
    public Guid LessonId { get; set; }
    public Guid? StudentId { get; set; }
    public Guid? ChildId { get; set; }
    public AttendanceStatus Status { get; set; }
    public TimeSpan? ArrivalTime { get; set; }
    public string? ExcuseReason { get; set; }
}

public class MarkAttendanceHandler
{
    private readonly IDataContext _db;
    private readonly ICurrentUser _currentUser;

    public MarkAttendanceHandler(IDataContext db, ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<Result<AttendanceDto>> HandleAsync(MarkAttendanceRequest request, CancellationToken ct = default)
    {
        // Ищем существующую запись
        var existing = await _db.Attendances
            .FirstOrDefaultAsync(a => a.LessonId == request.LessonId &&
                ((request.StudentId.HasValue && a.StudentId == request.StudentId) ||
                 (request.ChildId.HasValue && a.ChildId == request.ChildId)), ct);

        if (existing != null)
        {
            // Обновляем
            existing.Status = request.Status;
            existing.ArrivalTime = request.ArrivalTime;
            existing.ExcuseReason = request.ExcuseReason;
            existing.MarkedAt = DateTime.UtcNow;
            existing.MarkedById = _currentUser.UserId ?? Guid.Empty;

            await _db.SaveChangesAsync(ct);

            return Result<AttendanceDto>.Success(new AttendanceDto
            {
                Id = existing.Id,
                LessonId = existing.LessonId,
                StudentId = existing.StudentId,
                ChildId = existing.ChildId,
                Status = existing.Status,
                ArrivalTime = existing.ArrivalTime,
                ExcuseReason = existing.ExcuseReason,
                MarkedAt = existing.MarkedAt
            });
        }

        // Создаём новую
        var attendance = new Attendance
        {
            Id = Guid.NewGuid(),
            LessonId = request.LessonId,
            StudentId = request.StudentId,
            ChildId = request.ChildId,
            Status = request.Status,
            ArrivalTime = request.ArrivalTime,
            ExcuseReason = request.ExcuseReason,
            MarkedById = _currentUser.UserId ?? Guid.Empty,
            MarkedAt = DateTime.UtcNow
        };

        _db.Add(attendance);
        await _db.SaveChangesAsync(ct);

        return Result<AttendanceDto>.Success(new AttendanceDto
        {
            Id = attendance.Id,
            LessonId = attendance.LessonId,
            StudentId = attendance.StudentId,
            ChildId = attendance.ChildId,
            Status = attendance.Status,
            ArrivalTime = attendance.ArrivalTime,
            ExcuseReason = attendance.ExcuseReason,
            MarkedAt = attendance.MarkedAt
        });
    }
}

// Массовая отметка посещаемости
public class BulkAttendanceItem
{
    public Guid? StudentId { get; set; }
    public Guid? ChildId { get; set; }
    public AttendanceStatus Status { get; set; }
}

public class BulkMarkAttendanceRequest
{
    public Guid LessonId { get; set; }
    public List<BulkAttendanceItem> Items { get; set; } = new();
}

public class BulkMarkAttendanceHandler
{
    private readonly IDataContext _db;
    private readonly ICurrentUser _currentUser;

    public BulkMarkAttendanceHandler(IDataContext db, ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<Result<int>> HandleAsync(BulkMarkAttendanceRequest request, CancellationToken ct = default)
    {
        var existing = await _db.Attendances
            .Where(a => a.LessonId == request.LessonId)
            .ToListAsync(ct);

        var now = DateTime.UtcNow;
        var userId = _currentUser.UserId ?? Guid.Empty;
        var count = 0;

        foreach (var item in request.Items)
        {
            var att = existing.FirstOrDefault(a =>
                (item.StudentId.HasValue && a.StudentId == item.StudentId) ||
                (item.ChildId.HasValue && a.ChildId == item.ChildId));

            if (att != null)
            {
                att.Status = item.Status;
                att.MarkedAt = now;
                att.MarkedById = userId;
            }
            else
            {
                _db.Add(new Attendance
                {
                    Id = Guid.NewGuid(),
                    LessonId = request.LessonId,
                    StudentId = item.StudentId,
                    ChildId = item.ChildId,
                    Status = item.Status,
                    MarkedById = userId,
                    MarkedAt = now
                });
            }
            count++;
        }

        await _db.SaveChangesAsync(ct);

        return Result<int>.Success(count);
    }
}
