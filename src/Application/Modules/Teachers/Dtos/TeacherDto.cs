using Domain.Teachers;

namespace Application.Modules.Teachers.Dtos;

// DTO учителя для списка
public class TeacherDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public TeacherStatus Status { get; set; }
    public List<string> Subjects { get; set; } = new();
    public TeacherPaymentType PaymentType { get; set; }
    public DateTime CreatedAt { get; set; }
}
