using Application.Abstractions;
using Application.Common;
using Application.Modules.Students.Dtos;
using Application.Modules.Students.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Students.UseCases.GetStudent;

// Handler получения студента по ID
public class GetStudentHandler
{
    private readonly IDataContext _db;

    public GetStudentHandler(IDataContext db)
    {
        _db = db;
    }

    public async Task<Result<StudentDetailDto>> HandleAsync(Guid id, CancellationToken ct = default)
    {
        var student = await _db.Students
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Id == id, ct);

        if (student == null)
            return Result<StudentDetailDto>.Failure(Error.NotFound("Студент"));

        return Result<StudentDetailDto>.Success(StudentMapper.ToDetailDto(student));
    }
}
