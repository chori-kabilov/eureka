using Application.Abstractions;
using Application.Common;
using Application.Modules.Groups.Dtos;
using Domain.Groups;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Groups.UseCases.TransferStudent;

// Перевод в другую группу
public class TransferStudentHandler(IDataContext db)
{
    public async Task<Result<EnrollmentDto>> HandleAsync(TransferStudentRequest request, CancellationToken ct = default)
    {
        var oldEnrollment = await db.GroupEnrollments.FirstOrDefaultAsync(e => e.Id == request.EnrollmentId, ct);
        if (oldEnrollment == null)
            return Result<EnrollmentDto>.Failure(Error.NotFound("Зачисление"));

        var newGroup = await db.Groups.FirstOrDefaultAsync(g => g.Id == request.NewGroupId, ct);
        if (newGroup == null)
            return Result<EnrollmentDto>.Failure(Error.NotFound("Новая группа"));

        oldEnrollment.Status = EnrollmentStatus.Transferred;
        oldEnrollment.LeftAt = DateTime.UtcNow;
        oldEnrollment.TransferredToGroupId = request.NewGroupId;

        var newEnrollment = new GroupEnrollment
        {
            Id = Guid.NewGuid(),
            GroupId = request.NewGroupId,
            StudentId = oldEnrollment.StudentId,
            ChildId = oldEnrollment.ChildId,
            EnrolledAt = DateTime.UtcNow,
            Status = EnrollmentStatus.Active,
            TransferredFromGroupId = oldEnrollment.GroupId,
            Notes = request.Notes
        };

        db.Add(newEnrollment);
        await db.SaveChangesAsync(ct);

        return Result<EnrollmentDto>.Success(new EnrollmentDto
        {
            Id = newEnrollment.Id,
            GroupId = newEnrollment.GroupId,
            StudentId = newEnrollment.StudentId,
            ChildId = newEnrollment.ChildId,
            EnrolledAt = newEnrollment.EnrolledAt,
            Status = newEnrollment.Status,
            Notes = newEnrollment.Notes
        });
    }
}
