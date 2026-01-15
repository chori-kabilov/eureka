using Application.Abstractions;
using Application.Common;
using Application.Modules.Courses.Dtos;
using Application.Modules.Courses.Mapping;
using Domain.Common;
using Domain.Courses;

namespace Application.Modules.Courses.UseCases.CreateCourse;

// Handler для создания курса
public class CreateCourseHandler(IDataContext db)
{
    public async Task<Result<CourseDetailDto>> HandleAsync(
        CreateCourseRequest request,
        CancellationToken ct = default)
    {
        // Валидация
        Guard.AgainstEmpty(request.Name, nameof(request.Name));

        // Создание сущности
        var course = new Course
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Description = request.Description?.Trim(),
            StudentPaymentType = request.StudentPaymentType,
            AbsencePolicy = request.AbsencePolicy,
            TeacherPaymentType = request.TeacherPaymentType,
            Status = CourseStatus.Active
        };

        db.Add(course);
        await db.SaveChangesAsync(ct);

        return Result<CourseDetailDto>.Success(CourseMapper.ToDetailDto(course));
    }
}
