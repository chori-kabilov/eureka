using Application.Abstractions;
using Application.Common;
using Application.Modules.Parents.Dtos;
using Application.Modules.Parents.Mapping;
using Domain.Parents;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Parents.UseCases.CreateParent;

// Request для создания родителя
public class CreateParentRequest
{
    public Guid UserId { get; set; }
}

// Handler создания родителя
public class CreateParentHandler
{
    private readonly IDataContext _db;

    public CreateParentHandler(IDataContext db)
    {
        _db = db;
    }

    public async Task<Result<ParentDto>> HandleAsync(
        CreateParentRequest request,
        CancellationToken ct = default)
    {
        // Проверка: пользователь существует
        var user = await _db.Users
            .Include(u => u.ParentProfile)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, ct);

        if (user == null)
            return Result<ParentDto>.Failure(Error.NotFound("Пользователь"));

        // Проверка: уже родитель
        if (user.ParentProfile != null)
            return Result<ParentDto>.Failure(
                Error.Conflict("Пользователь уже является родителем"));

        // Создание родителя
        var parent = new Parent
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId
        };

        _db.Add(parent);
        await _db.SaveChangesAsync(ct);

        // Загрузка связей
        parent.User = user;

        return Result<ParentDto>.Success(ParentMapper.ToDto(parent));
    }
}
