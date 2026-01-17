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

        Console.WriteLine($"UPDATE: FullName='{request.FullName}', Phone='{request.Phone}', Status={request.Status}, Notes='{request.Notes}'");

        if (student == null)
            return Result<StudentDetailDto>.Failure(Error.NotFound("Студент"));

        if (!string.IsNullOrEmpty(request.FullName) && student.User != null)
            student.User.FullName = request.FullName;
        
        if (!string.IsNullOrEmpty(request.Phone) && student.User != null)
            student.User.Phone = request.Phone;

        // Помечаем User как изменённый
        if (student.User != null)
            student.User.UpdatedAt = DateTime.UtcNow;

        if (request.Status.HasValue)
            student.Status = (StudentStatus)request.Status.Value;
        
        // Всегда обновляем Notes (пустая строка = null)
        student.Notes = string.IsNullOrWhiteSpace(request.Notes) ? null : request.Notes;

        student.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync(ct);

        return Result<StudentDetailDto>.Success(StudentMapper.ToDetailDto(student));
    }
}
