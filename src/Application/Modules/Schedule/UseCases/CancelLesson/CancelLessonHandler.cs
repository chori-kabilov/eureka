using Application.Abstractions;
using Application.Common;
using Domain.Schedule;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Schedule.UseCases.CancelLesson;

// Отменить занятие
public class CancelLessonHandler(IDataContext db)
{
    public async Task<Result<bool>> HandleAsync(Guid lessonId, string? reason, CancellationToken ct = default)
    {
        var lesson = await db.Lessons.FirstOrDefaultAsync(l => l.Id == lessonId, ct);
        if (lesson == null)
            return Result<bool>.Failure(Error.NotFound("Занятие"));

        lesson.Status = LessonStatus.Cancelled;
        lesson.CancellationReason = reason;

        await db.SaveChangesAsync(ct);

        return Result<bool>.Success(true);
    }
}
