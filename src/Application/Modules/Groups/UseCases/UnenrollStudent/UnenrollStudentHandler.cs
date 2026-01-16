using Application.Abstractions;
using Application.Common;
using Domain.Groups;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Groups.UseCases.UnenrollStudent;

// Отчислить из группы
public class UnenrollStudentHandler(IDataContext db)
{
    public async Task<Result<bool>> HandleAsync(Guid enrollmentId, EnrollmentStatus newStatus = EnrollmentStatus.Expelled, CancellationToken ct = default)
    {
        var enrollment = await db.GroupEnrollments.FirstOrDefaultAsync(e => e.Id == enrollmentId, ct);
        if (enrollment == null)
            return Result<bool>.Failure(Error.NotFound("Зачисление"));

        enrollment.Status = newStatus;
        enrollment.LeftAt = DateTime.UtcNow;

        await db.SaveChangesAsync(ct);

        return Result<bool>.Success(true);
    }
}
