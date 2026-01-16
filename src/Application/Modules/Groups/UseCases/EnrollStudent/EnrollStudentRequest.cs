namespace Application.Modules.Groups.UseCases.EnrollStudent;

// Запрос на зачисление студента/ребёнка в группу
public class EnrollStudentRequest
{
    public Guid GroupId { get; set; }
    public Guid? StudentId { get; set; }
    public Guid? ChildId { get; set; }
    public string? Notes { get; set; }
}
