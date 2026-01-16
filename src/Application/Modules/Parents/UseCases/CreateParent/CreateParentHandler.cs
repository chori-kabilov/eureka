using Application.Abstractions;
using Application.Common;
using Application.Modules.Parents.Dtos;
using Application.Modules.Parents.Mapping;
using Domain.Parents;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Parents.UseCases.CreateParent;

// Создать родителя
public class CreateParentHandler(IDataContext db)
{
    public async Task<Result<ParentDto>> HandleAsync(
        CreateParentRequest request,
        CancellationToken ct = default)
    {
        var user = await db.Users
            .Include(u => u.ParentProfile)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, ct);

        if (user == null)
            return Result<ParentDto>.Failure(Error.NotFound("Пользователь"));

        if (user.ParentProfile != null)
            return Result<ParentDto>.Failure(
                Error.Conflict("Пользователь уже является родителем"));

        var parent = new Parent
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId
        };

        db.Add(parent);
        await db.SaveChangesAsync(ct);

        parent.User = user;

        return Result<ParentDto>.Success(ParentMapper.ToDto(parent));
    }
}
