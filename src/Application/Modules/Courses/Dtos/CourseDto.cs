using Domain.Courses;

namespace Application.Modules.Courses.Dtos;

// DTO для списка курсов
public class CourseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public StudentPaymentType StudentPaymentType { get; set; }
    public TeacherPaymentType TeacherPaymentType { get; set; }
    public AbsencePolicy AbsencePolicy { get; set; }
    public CourseStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

