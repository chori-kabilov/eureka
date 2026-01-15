using Application.Abstractions;
using Application.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Parents.UseCases.DeleteParent;

// Handler удаления родителя
public class DeleteParentHandler
{
    private readonly IDataContext _db;

    public DeleteParentHandler(IDataContext db)
    {
        _db = db;
    }

    public async Task<Result> HandleAsync(Guid id, CancellationToken ct = default)
    {
        var parent = await _db.Parents.FirstOrDefaultAsync(p => p.Id == id, ct);

        if (parent == null)
            return Result.Failure(Error.NotFound("Родитель"));

        _db.Remove(parent);
        await _db.SaveChangesAsync(ct);

        return Result.Success();
    }
}
