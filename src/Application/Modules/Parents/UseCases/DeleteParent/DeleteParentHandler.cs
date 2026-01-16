using Application.Abstractions;
using Application.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Parents.UseCases.DeleteParent;

// Удалить родителя
public class DeleteParentHandler(IDataContext db)
{
    public async Task<Result> HandleAsync(Guid id, CancellationToken ct = default)
    {
        var parent = await db.Parents.FirstOrDefaultAsync(p => p.Id == id, ct);

        if (parent == null)
            return Result.Failure(Error.NotFound("Родитель"));

        db.Remove(parent);
        await db.SaveChangesAsync(ct);

        return Result.Success();
    }
}
