using Application.Abstractions;
using Application.Common;
using Domain.Journal;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Journal.UseCases.BulkMarkAttendance;

// Массовая отметка посещаемости
public class BulkMarkAttendanceHandler(IDataContext db, ICurrentUser currentUser)
{
    public async Task<Result<int>> HandleAsync(BulkMarkAttendanceRequest request, CancellationToken ct = default)
    {
        var existing = await db.Attendances
            .Where(a => a.LessonId == request.LessonId)
            .ToListAsync(ct);

        var now = DateTime.UtcNow;
        var userId = currentUser.UserId ?? Guid.Empty;
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
                db.Add(new Attendance
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

        await db.SaveChangesAsync(ct);

        return Result<int>.Success(count);
    }
}
