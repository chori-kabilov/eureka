using Application.Modules.Courses.Dtos;
using Domain.Courses;

namespace Application.Modules.Courses.Mapping;

// Manual mapping для курсов
public static class CourseMapper
{
    public static CourseDto ToDto(Course course)
    {
        return new CourseDto
        {
            Id = course.Id,
            Name = course.Name,
            Description = course.Description,
            StudentPaymentType = course.StudentPaymentType,
            TeacherPaymentType = course.TeacherPaymentType,
            AbsencePolicy = course.AbsencePolicy,
            Status = course.Status,
            CreatedAt = course.CreatedAt
        };
    }

    public static CourseDetailDto ToDetailDto(Course course)
    {
        return new CourseDetailDto
        {
            Id = course.Id,
            Name = course.Name,
            Description = course.Description,
            StudentPaymentType = course.StudentPaymentType,
            TeacherPaymentType = course.TeacherPaymentType,
            AbsencePolicy = course.AbsencePolicy,
            Status = course.Status,
            CreatedAt = course.CreatedAt,
            CreatedBy = course.CreatedBy,
            UpdatedAt = course.UpdatedAt,
            UpdatedBy = course.UpdatedBy
        };
    }

    public static List<CourseDto> ToDtoList(IEnumerable<Course> courses)
    {
        return courses.Select(ToDto).ToList();
    }
}
