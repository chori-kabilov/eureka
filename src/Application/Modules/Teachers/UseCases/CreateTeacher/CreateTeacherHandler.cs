using Application.Abstractions;
using Application.Common;
using Application.Modules.Teachers.Dtos;
using Application.Modules.Teachers.Mapping;
using Domain.Teachers;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Teachers.UseCases.CreateTeacher;

// Request для создания учителя
public class CreateTeacherRequest
{
    public Guid UserId { get; set; }
    public string? Specialization { get; set; }
    public int PaymentType { get; set; }
    public decimal? HourlyRate { get; set; }
    public string? Bio { get; set; }
}

// Handler создания учителя
public class CreateTeacherHandler
{
    private readonly IDataContext _db;

    public CreateTeacherHandler(IDataContext db)
    {
        _db = db;
    }

    public async Task<Result<TeacherDetailDto>> HandleAsync(
        CreateTeacherRequest request,
        CancellationToken ct = default)
    {
        // Проверка: пользователь существует
        var user = await _db.Users
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

        _db.Add(teacher);
        await _db.SaveChangesAsync(ct);

        // Загрузка связей
        teacher.User = user;

        return Result<TeacherDetailDto>.Success(TeacherMapper.ToDetailDto(teacher));
    }
}
