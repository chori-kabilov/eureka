using Domain.Courses;

namespace Application.Modules.Courses.UseCases.UpdateCourse;

// Запрос на обновление курса
public class UpdateCourseRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public StudentPaymentType StudentPaymentType { get; set; }
    public AbsencePolicy AbsencePolicy { get; set; }
    public TeacherPaymentType TeacherPaymentType { get; set; }
}
