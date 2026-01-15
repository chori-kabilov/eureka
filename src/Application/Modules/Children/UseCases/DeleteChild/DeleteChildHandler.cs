using Application.Abstractions;
using Application.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Children.UseCases.DeleteChild;

// Handler удаления ребёнка
public class DeleteChildHandler
{
    private readonly IDataContext _db;

    public DeleteChildHandler(IDataContext db)
    {
        _db = db;
    }

    public async Task<Result> HandleAsync(Guid id, CancellationToken ct = default)
    {
        var child = await _db.Children.FirstOrDefaultAsync(c => c.Id == id, ct);

        if (child == null)
            return Result.Failure(Error.NotFound("Ребёнок"));

        _db.Remove(child);
        await _db.SaveChangesAsync(ct);

        return Result.Success();
    }
}
