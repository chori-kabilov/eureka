using Application.Abstractions;
using Application.Common;
using Application.Modules.Courses.Dtos;
using Application.Modules.Courses.Mapping;
using Domain.Common;
using Domain.Courses;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Courses.UseCases.UpdateCourse;

// Handler для обновления курса
public class UpdateCourseHandler(IDataContext db)
{
    public async Task<Result<CourseDetailDto>> HandleAsync(
        UpdateCourseRequest request,
        CancellationToken ct = default)
    {
        // Валидация
        Guard.AgainstEmpty(request.Name, nameof(request.Name));

        // Поиск курса
        var course = await db.Courses
            .FirstOrDefaultAsync(c => c.Id == request.Id, ct);

        if (course is null)
            return Result<CourseDetailDto>.Failure(Error.NotFound("Курс"));

        // Обновление
        course.Name = request.Name.Trim();
        course.Description = request.Description?.Trim();
        course.StudentPaymentType = request.StudentPaymentType;
        course.AbsencePolicy = request.AbsencePolicy;
        course.TeacherPaymentType = request.TeacherPaymentType;

        db.Update(course);
        await db.SaveChangesAsync(ct);

        return Result<CourseDetailDto>.Success(CourseMapper.ToDetailDto(course));
    }
}
