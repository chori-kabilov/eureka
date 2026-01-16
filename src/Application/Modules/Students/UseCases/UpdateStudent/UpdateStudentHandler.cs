using Application.Abstractions;
using Application.Common;
using Application.Modules.Students.Dtos;
using Application.Modules.Students.Mapping;
using Domain.Students;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Students.UseCases.UpdateStudent;

// Handler обновления студента
public class UpdateStudentHandler(IDataContext db)
{
    public async Task<Result<StudentDetailDto>> HandleAsync(
        UpdateStudentRequest request,
        CancellationToken ct = default)
    {
        var student = await db.Students
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Id == request.Id, ct);

        if (student == null)
            return Result<StudentDetailDto>.Failure(Error.NotFound("Студент"));

        if (request.Status.HasValue)
            student.Status = (StudentStatus)request.Status.Value;
        
        if (request.Notes != null)
            student.Notes = request.Notes;

        student.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync(ct);

        return Result<StudentDetailDto>.Success(StudentMapper.ToDetailDto(student));
    }
}
