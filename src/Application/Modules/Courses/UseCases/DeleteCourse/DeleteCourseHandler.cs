using Application.Abstractions;
using Application.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Courses.UseCases.DeleteCourse;

// Handler для soft delete курса
public class DeleteCourseHandler(IDataContext db)
{
    public async Task<Result> HandleAsync(Guid id, CancellationToken ct = default)
    {
        var course = await db.Courses
            .FirstOrDefaultAsync(c => c.Id == id, ct);

        if (course is null)
            return Result.Failure(Error.NotFound("Курс"));

        // Soft delete — Remove вызывает логику в AppDbContext.SaveChangesAsync
        db.Remove(course);
        await db.SaveChangesAsync(ct);

        return Result.Success();
    }
}
