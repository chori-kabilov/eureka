using Application.Abstractions;
using Application.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Students.UseCases.DeleteStudent;

// Handler удаления студента
public class DeleteStudentHandler(IDataContext db)
{
    public async Task<Result> HandleAsync(Guid id, CancellationToken ct = default)
    {
        var student = await db.Students.FirstOrDefaultAsync(s => s.Id == id, ct);

        if (student == null)
            return Result.Failure(Error.NotFound("Студент"));

        db.Remove(student);
        await db.SaveChangesAsync(ct);

        return Result.Success();
    }
}
