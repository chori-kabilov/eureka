using Domain.Students;

namespace Application.Modules.Children.Dtos;

// DTO ребёнка для списка
public class ChildDto
{
    public Guid Id { get; set; }
    public Guid ParentId { get; set; }
    public string ParentName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public StudentStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Детальный DTO ребёнка
public class ChildDetailDto : ChildDto
{
    public string? Notes { get; set; }
    public Guid? LinkedStudentId { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

// DTO для создания ребёнка
public class CreateChildDto
{
    public Guid ParentId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public string? Notes { get; set; }
}
