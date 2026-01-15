namespace RazorWebApp.Models.Users;

// ViewModel пользователя
public class UserViewModel
{
    public Guid Id { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public bool IsStudent { get; set; }
    public bool IsTeacher { get; set; }
    public bool IsParent { get; set; }
    public DateTime CreatedAt { get; set; }

    public string RoleName
    {
        get
        {
            var roles = new List<string>();
            if (IsAdmin) roles.Add("Admin");
            if (IsTeacher) roles.Add("Teacher");
            if (IsStudent) roles.Add("Student");
            if (IsParent) roles.Add("Parent");
            return roles.Count > 0 ? string.Join(", ", roles) : "User";
        }
    }
}

// Список пользователей с пагинацией
public class UsersPagedResponse
{
    public bool Success { get; set; }
    public List<UserViewModel> Items { get; set; } = new();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}
