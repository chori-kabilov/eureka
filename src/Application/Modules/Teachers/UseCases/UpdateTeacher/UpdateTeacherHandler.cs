using Application.Abstractions;
using Application.Common;
using Application.Modules.Teachers.Dtos;
using Application.Modules.Teachers.Mapping;
using Domain.Teachers;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Teachers.UseCases.UpdateTeacher;

// Request для обновления учителя
public class UpdateTeacherRequest
{
    public Guid Id { get; set; }
    public string? Specialization { get; set; }
    public int? PaymentType { get; set; }
    public decimal? HourlyRate { get; set; }
    public string? Bio { get; set; }
}

// Handler обновления учителя
public class UpdateTeacherHandler
{
    private readonly IDataContext _db;

    public UpdateTeacherHandler(IDataContext db)
    {
        _db = db;
    }

    public async Task<Result<TeacherDetailDto>> HandleAsync(
        UpdateTeacherRequest request,
        CancellationToken ct = default)
    {
        var teacher = await _db.Teachers
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Id == request.Id, ct);

        if (teacher == null)
            return Result<TeacherDetailDto>.Failure(Error.NotFound("Учитель"));

        if (request.Specialization != null)
            teacher.Specialization = request.Specialization;
        
        if (request.PaymentType.HasValue)
            teacher.PaymentType = (TeacherPaymentType)request.PaymentType.Value;
        
        if (request.HourlyRate.HasValue)
            teacher.HourlyRate = request.HourlyRate.Value;
        
        if (request.Bio != null)
            teacher.Bio = request.Bio;

        teacher.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);

        return Result<TeacherDetailDto>.Success(TeacherMapper.ToDetailDto(teacher));
    }
}
