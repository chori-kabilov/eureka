using Domain.Admins;
using Domain.Courses;
using Domain.Parents;
using Domain.Students;
using Domain.Teachers;
using Domain.Users;

namespace Application.Abstractions;

// Интерфейс для доступа к базе данных
public interface IDataContext
{
    // Пользователи
    IQueryable<User> Users { get; }
    
    // Профили
    IQueryable<Admin> Admins { get; }
    IQueryable<Student> Students { get; }
    IQueryable<Teacher> Teachers { get; }
    IQueryable<Parent> Parents { get; }
    
    // Дети
    IQueryable<Child> Children { get; }
    
    // Курсы
    IQueryable<Course> Courses { get; }
    
    // Сохранение изменений
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    
    // Добавление сущности
    void Add<T>(T entity) where T : class;
    
    // Обновление сущности
    void Update<T>(T entity) where T : class;
    
    // Удаление сущности
    void Remove<T>(T entity) where T : class;
}
