namespace Application.Modules.Students.Dtos;

// DTO для создания студента
public class CreateStudentDto
{
    public Guid UserId { get; set; }
    public string? Notes { get; set; }
}
