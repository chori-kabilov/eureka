namespace Domain.Users;

// Роли пользователей
public enum Role
{
    User = 0,       // По умолчанию при регистрации
    Admin = 1,      // Полный доступ
    Teacher = 2,    // Преподаватель
    Student = 3,    // Студент
    Parent = 4      // Родитель
}
