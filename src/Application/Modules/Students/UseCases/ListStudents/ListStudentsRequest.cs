namespace Application.Modules.Students.UseCases.ListStudents;

// Запрос на список студентов
public class ListStudentsRequest
{
    public string? Search { get; set; }
    public int? Status { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
