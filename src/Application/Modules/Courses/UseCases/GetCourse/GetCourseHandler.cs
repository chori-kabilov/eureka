using Application.Abstractions;
using Application.Common;
using Application.Modules.Courses.Dtos;
using Application.Modules.Courses.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Courses.UseCases.GetCourse;

// Handler для получения курса по ID
public class GetCourseHandler(IDataContext db)
{
    public async Task<Result<CourseDetailDto>> HandleAsync(Guid id, CancellationToken ct = default)
    {
        var course = await db.Courses
            .FirstOrDefaultAsync(c => c.Id == id, ct);

        if (course is null)
            return Result<CourseDetailDto>.Failure(Error.NotFound("Курс"));

        return Result<CourseDetailDto>.Success(CourseMapper.ToDetailDto(course));
    }
}
