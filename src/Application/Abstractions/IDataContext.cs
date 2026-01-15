using Domain.Courses;
using Domain.Users;

namespace Application.Abstractions;

// Интерфейс для доступа к базе данных
public interface IDataContext
{
    // Курсы
    IQueryable<Course> Courses { get; }
    
    // Пользователи
    IQueryable<User> Users { get; }
    
    // Сохранение изменений
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    
    // Добавление сущности
    void Add<T>(T entity) where T : class;
    
    // Обновление сущности
    void Update<T>(T entity) where T : class;
    
    // Удаление сущности
    void Remove<T>(T entity) where T : class;
}
