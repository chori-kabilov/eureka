using Application.Abstractions;
using Application.Common;
using Microsoft.EntityFrameworkCore;
using Domain.Courses;

namespace Application.Modules.Courses.UseCases.ArchiveCourse;

// Handler для архивации курса
public class ArchiveCourseHandler(IDataContext db)
{
    public async Task<Result> HandleAsync(Guid id, CancellationToken ct = default)
    {
        var course = await db.Courses
            .FirstOrDefaultAsync(c => c.Id == id, ct);

        if (course is null)
            return Result.Failure(Error.NotFound("Курс"));

        course.Status = CourseStatus.Archived;

        db.Update(course);
        await db.SaveChangesAsync(ct);

        return Result.Success();
    }
}
