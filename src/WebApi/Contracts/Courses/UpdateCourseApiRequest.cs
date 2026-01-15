using Domain.Courses;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Contracts.Courses;

// API контракт для обновления курса
public class UpdateCourseApiRequest
{
    [Required(ErrorMessage = "Название обязательно")]
    [MaxLength(200, ErrorMessage = "Название не более 200 символов")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(2000, ErrorMessage = "Описание не более 2000 символов")]
    public string? Description { get; set; }

    public StudentPaymentType StudentPaymentType { get; set; }
    public AbsencePolicy AbsencePolicy { get; set; }
    public TeacherPaymentType TeacherPaymentType { get; set; }
}
