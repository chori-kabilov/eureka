using Application.Abstractions;
using Application.Common;
using Application.Modules.Students.Dtos;
using Application.Modules.Students.Mapping;
using Domain.Students;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Students.UseCases.UpdateStudent;

// Request для обновления студента
public class UpdateStudentRequest
{
    public Guid Id { get; set; }
    public int? Status { get; set; }
    public string? Notes { get; set; }
}

// Handler обновления студента
public class UpdateStudentHandler
{
    private readonly IDataContext _db;

    public UpdateStudentHandler(IDataContext db)
    {
        _db = db;
    }

    public async Task<Result<StudentDetailDto>> HandleAsync(
        UpdateStudentRequest request,
        CancellationToken ct = default)
    {
        var student = await _db.Students
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Id == request.Id, ct);

        if (student == null)
            return Result<StudentDetailDto>.Failure(Error.NotFound("Студент"));

        if (request.Status.HasValue)
            student.Status = (StudentStatus)request.Status.Value;
        
        if (request.Notes != null)
            student.Notes = request.Notes;

        student.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);

        return Result<StudentDetailDto>.Success(StudentMapper.ToDetailDto(student));
    }
}
