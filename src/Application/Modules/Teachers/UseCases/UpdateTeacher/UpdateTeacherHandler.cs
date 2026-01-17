using Application.Abstractions;
using Application.Common;
using Application.Modules.Teachers.Dtos;
using Application.Modules.Teachers.Mapping;
using Domain.Teachers;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Teachers.UseCases.UpdateTeacher;

// Handler обновления учителя
public class UpdateTeacherHandler(IDataContext db)
{
    public async Task<Result<TeacherDetailDto>> HandleAsync(
        UpdateTeacherRequest request,
        CancellationToken ct = default)
    {
        var teacher = await db.Teachers
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Id == request.Id, ct);

        if (teacher == null)
            return Result<TeacherDetailDto>.Failure(Error.NotFound("Учитель"));

        if (!string.IsNullOrEmpty(request.FullName) && teacher.User != null)
            teacher.User.FullName = request.FullName;
        
        if (!string.IsNullOrEmpty(request.Phone) && teacher.User != null)
            teacher.User.Phone = request.Phone;

        if (teacher.User != null)
            teacher.User.UpdatedAt = DateTime.UtcNow;

        if (request.Status.HasValue)
            teacher.Status = (TeacherStatus)request.Status.Value;

        if (request.Subjects != null)
            teacher.Subjects = request.Subjects;
        
        if (request.PaymentType.HasValue)
            teacher.PaymentType = (TeacherPaymentType)request.PaymentType.Value;
        
        if (request.HourlyRate.HasValue)
            teacher.HourlyRate = request.HourlyRate.Value;
        
        if (request.Bio != null)
            teacher.Bio = request.Bio;

        teacher.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync(ct);

        return Result<TeacherDetailDto>.Success(TeacherMapper.ToDetailDto(teacher));
    }
}
