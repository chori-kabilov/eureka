using Domain.Common;

namespace Domain.Courses;

// Сущность курса
public class Course : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public StudentPaymentType StudentPaymentType { get; set; }
    public AbsencePolicy AbsencePolicy { get; set; }
    public TeacherPaymentType TeacherPaymentType { get; set; }
    public CourseStatus Status { get; set; }
}
