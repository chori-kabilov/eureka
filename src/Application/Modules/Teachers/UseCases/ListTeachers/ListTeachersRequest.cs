namespace Application.Modules.Teachers.UseCases.ListTeachers;

// Запрос на список учителей
public class ListTeachersRequest
{
    public string? Search { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
