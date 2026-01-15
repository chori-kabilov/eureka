using Application.Abstractions;
using Domain.Admins;
using Domain.Common;
using Domain.Courses;
using Domain.Parents;
using Domain.Students;
using Domain.Teachers;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

// Реализация DbContext для PostgreSQL
public class DataContext(
    DbContextOptions<DataContext> options,
    ICurrentUser currentUser)
    : DbContext(options), IDataContext
{
    public DbSet<User> UsersSet { get; set; } = null!;
    public DbSet<Admin> AdminsSet { get; set; } = null!;
    public DbSet<Student> StudentsSet { get; set; } = null!;
    public DbSet<Teacher> TeachersSet { get; set; } = null!;
    public DbSet<Parent> ParentsSet { get; set; } = null!;
    public DbSet<Child> ChildrenSet { get; set; } = null!;
    public DbSet<Course> CoursesSet { get; set; } = null!;

    // IDataContext реализация
    IQueryable<User> IDataContext.Users => UsersSet;
    IQueryable<Admin> IDataContext.Admins => AdminsSet;
    IQueryable<Student> IDataContext.Students => StudentsSet;
    IQueryable<Teacher> IDataContext.Teachers => TeachersSet;
    IQueryable<Parent> IDataContext.Parents => ParentsSet;
    IQueryable<Child> IDataContext.Children => ChildrenSet;
    IQueryable<Course> IDataContext.Courses => CoursesSet;

    void IDataContext.Add<T>(T entity) => Set<T>().Add(entity);
    void IDataContext.Update<T>(T entity) => Set<T>().Update(entity);
    void IDataContext.Remove<T>(T entity) => Set<T>().Remove(entity);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Применяем все конфигурации из текущей сборки
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

        // Глобальные фильтры для soft delete
        modelBuilder.Entity<User>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Admin>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Student>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Teacher>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Parent>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Child>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Course>().HasQueryFilter(e => !e.IsDeleted);
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
