namespace Application.Modules.Parents.UseCases.ListParents;

// Запрос на список родителей
public class ListParentsRequest
{
    public string? Search { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
