using Application.Abstractions;
using Application.Common;
using Application.Modules.Children.Dtos;
using Application.Modules.Children.Mapping;
using Domain.Students;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Children.UseCases.CreateChild;

// Handler создания ребёнка
public class CreateChildHandler(IDataContext db)
{
    public async Task<Result<ChildDetailDto>> HandleAsync(
        CreateChildRequest request,
        CancellationToken ct = default)
    {
        // Проверка: родитель существует
        var parent = await db.Parents
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

        db.Add(child);
        await db.SaveChangesAsync(ct);

        // Загрузка связей
        child.Parent = parent;

        return Result<ChildDetailDto>.Success(ChildMapper.ToDetailDto(child));
    }
}
