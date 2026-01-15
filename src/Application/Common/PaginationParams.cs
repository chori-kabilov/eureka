namespace Application.Common;

// Параметры пагинации
public class PaginationParams
{
    private const int MaxPageSize = 100;
    private const int DefaultPageSize = 20;

    public int Page { get; set; } = 1;

    private int _pageSize = DefaultPageSize;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    public int Skip => (Page - 1) * PageSize;
}
