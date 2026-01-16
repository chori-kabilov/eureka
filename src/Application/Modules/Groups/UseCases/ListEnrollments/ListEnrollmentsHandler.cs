using Application.Abstractions;
using Application.Common;
using Application.Modules.Groups.Dtos;
using Domain.Groups;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Groups.UseCases.ListEnrollments;

// Получить список студентов группы
public class ListEnrollmentsHandler(IDataContext db)
{
    public async Task<Result<List<EnrollmentDto>>> HandleAsync(Guid groupId, EnrollmentStatus? status = null, CancellationToken ct = default)
    {
        var query = db.GroupEnrollments
            .Include(e => e.Student).ThenInclude(s => s!.User)
            .Include(e => e.Child).ThenInclude(c => c!.Parent).ThenInclude(p => p!.User)
            .Where(e => e.GroupId == groupId);

        if (status.HasValue)
            query = query.Where(e => e.Status == status);

        var enrollments = await query
            .OrderBy(e => e.Student != null ? e.Student.User!.FullName : e.Child!.FullName)
            .ToListAsync(ct);

        var items = enrollments.Select(e => new EnrollmentDto
        {
            Id = e.Id,
            GroupId = e.GroupId,
            StudentId = e.StudentId,
            StudentName = e.Student?.User?.FullName,
            StudentPhone = e.Student?.User?.Phone,
            ChildId = e.ChildId,
            ChildName = e.Child?.FullName,
            ParentName = e.Child?.Parent?.User?.FullName,
            EnrolledAt = e.EnrolledAt,
            LeftAt = e.LeftAt,
            Status = e.Status,
            Notes = e.Notes
        }).ToList();

        return Result<List<EnrollmentDto>>.Success(items);
    }
}
