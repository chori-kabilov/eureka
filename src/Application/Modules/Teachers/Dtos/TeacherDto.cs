using Domain.Teachers;

namespace Application.Modules.Teachers.Dtos;

// DTO учителя для списка
public class TeacherDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Specialization { get; set; }
    public TeacherPaymentType PaymentType { get; set; }
    public decimal? HourlyRate { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Детальный DTO учителя
public class TeacherDetailDto : TeacherDto
{
    public string? Bio { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

// DTO для создания учителя
public class CreateTeacherDto
{
    public Guid UserId { get; set; }
    public string? Specialization { get; set; }
    public int PaymentType { get; set; }
    public decimal? HourlyRate { get; set; }
    public string? Bio { get; set; }
}
