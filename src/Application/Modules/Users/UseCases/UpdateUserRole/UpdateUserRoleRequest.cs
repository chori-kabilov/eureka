namespace Application.Modules.Users.UseCases.UpdateUserRole;

// Запрос на изменение статуса Admin
public class UpdateUserRoleRequest
{
    public Guid UserId { get; set; }
    public bool IsAdmin { get; set; }
}
