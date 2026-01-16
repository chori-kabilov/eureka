using Application.Abstractions;
using Application.Common;
using Application.Modules.Journal.Dtos;
using Domain.Journal;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Journal.UseCases.MarkAttendance;

// Отметить посещаемость
public class MarkAttendanceHandler(IDataContext db, ICurrentUser currentUser)
{
    public async Task<Result<AttendanceDto>> HandleAsync(MarkAttendanceRequest request, CancellationToken ct = default)
    {
        var existing = await db.Attendances
            .FirstOrDefaultAsync(a => a.LessonId == request.LessonId &&
                ((request.StudentId.HasValue && a.StudentId == request.StudentId) ||
                 (request.ChildId.HasValue && a.ChildId == request.ChildId)), ct);

        if (existing != null)
        {
            existing.Status = request.Status;
            existing.ArrivalTime = request.ArrivalTime;
            existing.ExcuseReason = request.ExcuseReason;
            existing.MarkedAt = DateTime.UtcNow;
            existing.MarkedById = currentUser.UserId ?? Guid.Empty;

            await db.SaveChangesAsync(ct);

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

        var attendance = new Attendance
        {
            Id = Guid.NewGuid(),
            LessonId = request.LessonId,
            StudentId = request.StudentId,
            ChildId = request.ChildId,
            Status = request.Status,
            ArrivalTime = request.ArrivalTime,
            ExcuseReason = request.ExcuseReason,
            MarkedById = currentUser.UserId ?? Guid.Empty,
            MarkedAt = DateTime.UtcNow
        };

        db.Add(attendance);
        await db.SaveChangesAsync(ct);

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
