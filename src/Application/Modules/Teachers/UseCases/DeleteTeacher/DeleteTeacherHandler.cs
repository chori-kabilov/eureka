using Application.Abstractions;
using Application.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Teachers.UseCases.DeleteTeacher;

// Handler удаления учителя
public class DeleteTeacherHandler
{
    private readonly IDataContext _db;

    public DeleteTeacherHandler(IDataContext db)
    {
        _db = db;
    }

    public async Task<Result> HandleAsync(Guid id, CancellationToken ct = default)
    {
        var teacher = await _db.Teachers.FirstOrDefaultAsync(t => t.Id == id, ct);

        if (teacher == null)
            return Result.Failure(Error.NotFound("Учитель"));

        _db.Remove(teacher);
        await _db.SaveChangesAsync(ct);

        return Result.Success();
    }
}
