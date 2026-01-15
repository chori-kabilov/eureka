using Application.Abstractions;
using Application.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Students.UseCases.DeleteStudent;

// Handler удаления студента
public class DeleteStudentHandler
{
    private readonly IDataContext _db;

    public DeleteStudentHandler(IDataContext db)
    {
        _db = db;
    }

    public async Task<Result> HandleAsync(Guid id, CancellationToken ct = default)
    {
        var student = await _db.Students.FirstOrDefaultAsync(s => s.Id == id, ct);

        if (student == null)
            return Result.Failure(Error.NotFound("Студент"));

        _db.Remove(student);
        await _db.SaveChangesAsync(ct);

        return Result.Success();
    }
}
