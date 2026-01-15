using Application.Abstractions;
using Application.Common;
using Application.Modules.Teachers.Dtos;
using Application.Modules.Teachers.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Teachers.UseCases.GetTeacher;

// Handler получения учителя по ID
public class GetTeacherHandler
{
    private readonly IDataContext _db;

    public GetTeacherHandler(IDataContext db)
    {
        _db = db;
    }

    public async Task<Result<TeacherDetailDto>> HandleAsync(Guid id, CancellationToken ct = default)
    {
        var teacher = await _db.Teachers
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Id == id, ct);

        if (teacher == null)
            return Result<TeacherDetailDto>.Failure(Error.NotFound("Учитель"));

        return Result<TeacherDetailDto>.Success(TeacherMapper.ToDetailDto(teacher));
    }
}
