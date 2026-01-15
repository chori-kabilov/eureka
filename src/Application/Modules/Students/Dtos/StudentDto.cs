using Domain.Students;

namespace Application.Modules.Students.Dtos;

// DTO студента для списка
public class StudentDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public StudentStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Детальный DTO студента
public class StudentDetailDto : StudentDto
{
    public string? Notes { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

// DTO для создания студента
public class CreateStudentDto
{
    public Guid UserId { get; set; }
    public string? Notes { get; set; }
}
