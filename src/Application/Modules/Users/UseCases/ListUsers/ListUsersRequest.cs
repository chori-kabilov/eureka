namespace Application.Modules.Users.UseCases.ListUsers;

// Фильтр для списка пользователей
public class ListUsersRequest
{
    public string? Search { get; set; }
    public bool? IsAdmin { get; set; }
    public bool? HasStudent { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
