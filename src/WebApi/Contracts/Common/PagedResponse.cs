namespace WebApi.Contracts.Common;

// Постраничный ответ API
public class PagedResponse<T>
{
    public bool Success { get; set; } = true;
    public List<T> Items { get; set; } = new();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
