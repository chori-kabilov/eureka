using Application.Abstractions;
using Application.Common;
using Application.Modules.Teachers.Dtos;
using Application.Modules.Teachers.Mapping;
using Domain.Teachers;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Teachers.UseCases.CreateTeacher;

// Handler создания учителя
public class CreateTeacherHandler(IDataContext db)
{
    public async Task<Result<TeacherDetailDto>> HandleAsync(
        CreateTeacherRequest request,
        CancellationToken ct = default)
    {
        // Проверка: пользователь существует
        var user = await db.Users
            .Include(u => u.TeacherProfile)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, ct);

        if (user == null)
            return Result<TeacherDetailDto>.Failure(Error.NotFound("Пользователь"));

        // Проверка: уже учитель
        if (user.TeacherProfile != null)
            return Result<TeacherDetailDto>.Failure(
                Error.Conflict("Пользователь уже является учителем"));

        // Создание учителя
        var teacher = new Teacher
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Specialization = request.Specialization,
            PaymentType = (TeacherPaymentType)request.PaymentType,
            HourlyRate = request.HourlyRate,
            Bio = request.Bio
        };

        db.Add(teacher);
        await db.SaveChangesAsync(ct);

        // Загрузка связей
        teacher.User = user;

        return Result<TeacherDetailDto>.Success(TeacherMapper.ToDetailDto(teacher));
    }
}
