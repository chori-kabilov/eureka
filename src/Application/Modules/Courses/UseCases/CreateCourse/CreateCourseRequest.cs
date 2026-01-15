using Domain.Courses;

namespace Application.Modules.Courses.UseCases.CreateCourse;

// Запрос на создание курса
public class CreateCourseRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public StudentPaymentType StudentPaymentType { get; set; } = StudentPaymentType.PerMonth;
    public AbsencePolicy AbsencePolicy { get; set; } = AbsencePolicy.Burn;
    public TeacherPaymentType TeacherPaymentType { get; set; } = TeacherPaymentType.PerLesson;
}
