using Domain.Courses;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Contracts.Courses;

// API контракт для создания курса
public class CreateCourseApiRequest
{
    [Required(ErrorMessage = "Название обязательно")]
    [MaxLength(200, ErrorMessage = "Название не более 200 символов")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(2000, ErrorMessage = "Описание не более 2000 символов")]
    public string? Description { get; set; }

    public StudentPaymentType StudentPaymentType { get; set; } = StudentPaymentType.PerMonth;
    public AbsencePolicy AbsencePolicy { get; set; } = AbsencePolicy.Burn;
    public TeacherPaymentType TeacherPaymentType { get; set; } = TeacherPaymentType.PerLesson;
}
