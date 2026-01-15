using Application.Abstractions;
using Domain.Common;
using Domain.Courses;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

// Реализация DbContext для PostgreSQL
public class DataContext(
    DbContextOptions<DataContext> options,
    ICurrentUser currentUser)
    : DbContext(options), IDataContext
{
    public DbSet<Course> CoursesSet { get; set; } = null!;
    public DbSet<User> UsersSet { get; set; } = null!;

    // IDataContext реализация
    IQueryable<Course> IDataContext.Courses => CoursesSet;
    IQueryable<User> IDataContext.Users => UsersSet;

    void IDataContext.Add<T>(T entity) => Set<T>().Add(entity);
    void IDataContext.Update<T>(T entity) => Set<T>().Update(entity);
    void IDataContext.Remove<T>(T entity) => Set<T>().Remove(entity);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Применяем все конфигурации из текущей сборки
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

        // Глобальные фильтры для soft delete
        modelBuilder.Entity<Course>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<User>().HasQueryFilter(e => !e.IsDeleted);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        // Автозаполнение полей аудита
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = now;
                    entry.Entity.CreatedBy = currentUser.UserId;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = now;
                    entry.Entity.UpdatedBy = currentUser.UserId;
                    break;
                case EntityState.Deleted:
                    // Soft delete вместо физического удаления
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedAt = now;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
