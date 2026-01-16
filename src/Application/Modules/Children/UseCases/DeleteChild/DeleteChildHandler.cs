using Application.Abstractions;
using Application.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Children.UseCases.DeleteChild;

// Handler удаления ребёнка
public class DeleteChildHandler(IDataContext db)
{
    public async Task<Result> HandleAsync(Guid id, CancellationToken ct = default)
    {
        var child = await db.Children.FirstOrDefaultAsync(c => c.Id == id, ct);

        if (child == null)
            return Result.Failure(Error.NotFound("Ребёнок"));

        db.Remove(child);
        await db.SaveChangesAsync(ct);

        return Result.Success();
    }
}
