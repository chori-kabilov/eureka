using Application.Abstractions;
using Application.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Teachers.UseCases.DeleteTeacher;

// Handler удаления учителя
public class DeleteTeacherHandler(IDataContext db)
{
    public async Task<Result> HandleAsync(Guid id, CancellationToken ct = default)
    {
        var teacher = await db.Teachers.FirstOrDefaultAsync(t => t.Id == id, ct);

        if (teacher == null)
            return Result.Failure(Error.NotFound("Учитель"));

        db.Remove(teacher);
        await db.SaveChangesAsync(ct);

        return Result.Success();
    }
}
