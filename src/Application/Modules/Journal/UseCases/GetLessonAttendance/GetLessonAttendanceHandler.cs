using Application.Abstractions;
using Application.Common;
using Application.Modules.Journal.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Journal.UseCases.GetLessonAttendance;

// Получить посещаемость занятия
public class GetLessonAttendanceHandler(IDataContext db)
{
    public async Task<Result<List<AttendanceDto>>> HandleAsync(Guid lessonId, CancellationToken ct = default)
    {
        var attendances = await db.Attendances
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
