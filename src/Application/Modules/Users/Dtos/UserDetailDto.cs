namespace Application.Modules.Users.Dtos;

// Детальный DTO пользователя
public class UserDetailDto : UserDto
{
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}
