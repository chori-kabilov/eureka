using Application.Abstractions;
using Application.Common;
using Application.Modules.Children.Dtos;
using Application.Modules.Children.Mapping;
using Domain.Students;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Children.UseCases.CreateChild;

// Request для создания ребёнка
public class CreateChildRequest
{
    public Guid ParentId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public string? Notes { get; set; }
}

// Handler создания ребёнка
public class CreateChildHandler
{
    private readonly IDataContext _db;

    public CreateChildHandler(IDataContext db)
    {
        _db = db;
    }

    public async Task<Result<ChildDetailDto>> HandleAsync(
        CreateChildRequest request,
        CancellationToken ct = default)
    {
        // Проверка: родитель существует
        var parent = await _db.Parents
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == request.ParentId, ct);

        if (parent == null)
            return Result<ChildDetailDto>.Failure(Error.NotFound("Родитель"));

        // Создание ребёнка
        DateTime? birthDateUtc = null;
        if (request.BirthDate.HasValue)
        {
            // Конвертируем в UTC для PostgreSQL
            birthDateUtc = DateTime.SpecifyKind(request.BirthDate.Value, DateTimeKind.Utc);
        }

        var child = new Child
        {
            Id = Guid.NewGuid(),
            ParentId = request.ParentId,
            FullName = request.FullName.Trim(),
            BirthDate = birthDateUtc,
            Status = StudentStatus.Active,
            Notes = request.Notes
        };

        _db.Add(child);
        await _db.SaveChangesAsync(ct);

        // Загрузка связей
        child.Parent = parent;

        return Result<ChildDetailDto>.Success(ChildMapper.ToDetailDto(child));
    }
}
