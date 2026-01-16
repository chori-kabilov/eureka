using Application.Abstractions;
using Application.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Groups.UseCases.DeleteGroup;

// Удалить группу
public class DeleteGroupHandler(IDataContext db)
{
    public async Task<Result<bool>> HandleAsync(Guid id, CancellationToken ct = default)
    {
        var group = await db.Groups.FirstOrDefaultAsync(g => g.Id == id, ct);
        if (group == null)
            return Result<bool>.Failure(Error.NotFound("Группа"));

        db.Remove(group);
        await db.SaveChangesAsync(ct);

        return Result<bool>.Success(true);
    }
}
