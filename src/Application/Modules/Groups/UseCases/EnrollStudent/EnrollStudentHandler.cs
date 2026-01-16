using Application.Abstractions;
using Application.Common;
using Application.Modules.Groups.Dtos;
using Domain.Groups;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Groups.UseCases.EnrollStudent;

// Зачислить студента/ребёнка в группу
public class EnrollStudentHandler(IDataContext db)
{
    public async Task<Result<EnrollmentDto>> HandleAsync(EnrollStudentRequest request, CancellationToken ct = default)
    {
        if (!request.StudentId.HasValue && !request.ChildId.HasValue)
            return Result<EnrollmentDto>.Failure(Error.Validation("Укажите студента или ребёнка"));

        var existing = await db.GroupEnrollments
            .FirstOrDefaultAsync(e => e.GroupId == request.GroupId && 
                e.Status == EnrollmentStatus.Active &&
                ((request.StudentId.HasValue && e.StudentId == request.StudentId) ||
                 (request.ChildId.HasValue && e.ChildId == request.ChildId)), ct);

        if (existing != null)
            return Result<EnrollmentDto>.Failure(Error.Conflict("Ученик уже в этой группе"));

        var group = await db.Groups.FirstOrDefaultAsync(g => g.Id == request.GroupId, ct);
        if (group == null)
            return Result<EnrollmentDto>.Failure(Error.NotFound("Группа"));

        var currentCount = await db.GroupEnrollments
            .CountAsync(e => e.GroupId == request.GroupId && e.Status == EnrollmentStatus.Active, ct);
        if (currentCount >= group.MaxStudents)
            return Result<EnrollmentDto>.Failure(Error.Validation("Группа заполнена"));

        var enrollment = new GroupEnrollment
        {
            Id = Guid.NewGuid(),
            GroupId = request.GroupId,
            StudentId = request.StudentId,
            ChildId = request.ChildId,
            EnrolledAt = DateTime.UtcNow,
            Status = EnrollmentStatus.Active,
            Notes = request.Notes
        };

        db.Add(enrollment);
        await db.SaveChangesAsync(ct);

        string? name = null;
        if (request.StudentId.HasValue)
        {
            var student = await db.Students.Include(s => s.User).FirstOrDefaultAsync(s => s.Id == request.StudentId, ct);
            name = student?.User?.FullName;
        }
        else if (request.ChildId.HasValue)
        {
            var child = await db.Children.FirstOrDefaultAsync(c => c.Id == request.ChildId, ct);
            name = child?.FullName;
        }

        return Result<EnrollmentDto>.Success(new EnrollmentDto
        {
            Id = enrollment.Id,
            GroupId = enrollment.GroupId,
            StudentId = enrollment.StudentId,
            StudentName = request.StudentId.HasValue ? name : null,
            ChildId = enrollment.ChildId,
            ChildName = request.ChildId.HasValue ? name : null,
            EnrolledAt = enrollment.EnrolledAt,
            Status = enrollment.Status,
            Notes = enrollment.Notes
        });
    }
}
