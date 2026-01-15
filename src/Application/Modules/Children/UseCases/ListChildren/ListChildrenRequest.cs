namespace Application.Modules.Children.UseCases.ListChildren;

// Запрос на список детей
public class ListChildrenRequest
{
    public string? Search { get; set; }
    public Guid? ParentId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
