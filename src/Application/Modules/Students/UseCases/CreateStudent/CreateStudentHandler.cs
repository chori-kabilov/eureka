using Application.Abstractions;
using Application.Common;
using Application.Modules.Students.Dtos;
using Application.Modules.Students.Mapping;
using Domain.Students;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Students.UseCases.CreateStudent;

// Request для создания студента
public class CreateStudentRequest
{
    public Guid UserId { get; set; }
    public string? Notes { get; set; }
}

// Handler создания студента
public class CreateStudentHandler(IDataContext db)
{
    public async Task<Result<StudentDetailDto>> HandleAsync(
        CreateStudentRequest request,
        CancellationToken ct = default)
    {
        // Проверка: пользователь существует
        var user = await db.Users
            .Include(u => u.StudentProfile)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, ct);

        if (user == null)
            return Result<StudentDetailDto>.Failure(Error.NotFound("Пользователь"));

        // Проверка: уже студент
        if (user.StudentProfile != null)
            return Result<StudentDetailDto>.Failure(
                Error.Conflict("Пользователь уже является студентом"));

        // Создание студента
        var student = new Student
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Balance = 0,
            Status = StudentStatus.Active,
            Notes = request.Notes
        };

        db.Add(student);
        await db.SaveChangesAsync(ct);

        // Загрузка связей
        student.User = user;

        return Result<StudentDetailDto>.Success(StudentMapper.ToDetailDto(student));
    }
}
