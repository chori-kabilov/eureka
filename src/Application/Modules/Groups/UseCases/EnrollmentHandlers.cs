using Application.Abstractions;
using Application.Common;
using Application.Modules.Groups.Dtos;
using Domain.Enums;
using Domain.Groups;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Groups.UseCases;

// Получить список студентов группы
public class ListEnrollmentsHandler
{
    private readonly IDataContext _db;

    public ListEnrollmentsHandler(IDataContext db) => _db = db;

    public async Task<Result<List<EnrollmentDto>>> HandleAsync(Guid groupId, EnrollmentStatus? status = null, CancellationToken ct = default)
    {
        var query = _db.GroupEnrollments
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

// Зачислить студента/ребёнка в группу
public class EnrollStudentRequest
{
    public Guid GroupId { get; set; }
    public Guid? StudentId { get; set; }
    public Guid? ChildId { get; set; }
    public string? Notes { get; set; }
}

public class EnrollStudentHandler
{
    private readonly IDataContext _db;

    public EnrollStudentHandler(IDataContext db) => _db = db;

    public async Task<Result<EnrollmentDto>> HandleAsync(EnrollStudentRequest request, CancellationToken ct = default)
    {
        if (!request.StudentId.HasValue && !request.ChildId.HasValue)
            return Result<EnrollmentDto>.Failure(Error.Validation("Укажите студента или ребёнка"));

        // Проверка: уже зачислен?
        var existing = await _db.GroupEnrollments
            .FirstOrDefaultAsync(e => e.GroupId == request.GroupId && 
                e.Status == EnrollmentStatus.Active &&
                ((request.StudentId.HasValue && e.StudentId == request.StudentId) ||
                 (request.ChildId.HasValue && e.ChildId == request.ChildId)), ct);

        if (existing != null)
            return Result<EnrollmentDto>.Failure(Error.Conflict("Ученик уже в этой группе"));

        // Проверка группы
        var group = await _db.Groups.FirstOrDefaultAsync(g => g.Id == request.GroupId, ct);
        if (group == null)
            return Result<EnrollmentDto>.Failure(Error.NotFound("Группа"));

        // Проверка лимита
        var currentCount = await _db.GroupEnrollments
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

        _db.Add(enrollment);
        await _db.SaveChangesAsync(ct);

        // Загрузка данных
        string? name = null;
        if (request.StudentId.HasValue)
        {
            var student = await _db.Students.Include(s => s.User).FirstOrDefaultAsync(s => s.Id == request.StudentId, ct);
            name = student?.User?.FullName;
        }
        else if (request.ChildId.HasValue)
        {
            var child = await _db.Children.FirstOrDefaultAsync(c => c.Id == request.ChildId, ct);
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

// Отчислить из группы
public class UnenrollStudentHandler
{
    private readonly IDataContext _db;

    public UnenrollStudentHandler(IDataContext db) => _db = db;

    public async Task<Result<bool>> HandleAsync(Guid enrollmentId, EnrollmentStatus newStatus = EnrollmentStatus.Expelled, CancellationToken ct = default)
    {
        var enrollment = await _db.GroupEnrollments.FirstOrDefaultAsync(e => e.Id == enrollmentId, ct);
        if (enrollment == null)
            return Result<bool>.Failure(Error.NotFound("Зачисление"));

        enrollment.Status = newStatus;
        enrollment.LeftAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);

        return Result<bool>.Success(true);
    }
}

// Перевод в другую группу
public class TransferStudentRequest
{
    public Guid EnrollmentId { get; set; }
    public Guid NewGroupId { get; set; }
    public string? Notes { get; set; }
}

public class TransferStudentHandler
{
    private readonly IDataContext _db;

    public TransferStudentHandler(IDataContext db) => _db = db;

    public async Task<Result<EnrollmentDto>> HandleAsync(TransferStudentRequest request, CancellationToken ct = default)
    {
        var oldEnrollment = await _db.GroupEnrollments.FirstOrDefaultAsync(e => e.Id == request.EnrollmentId, ct);
        if (oldEnrollment == null)
            return Result<EnrollmentDto>.Failure(Error.NotFound("Зачисление"));

        var newGroup = await _db.Groups.FirstOrDefaultAsync(g => g.Id == request.NewGroupId, ct);
        if (newGroup == null)
            return Result<EnrollmentDto>.Failure(Error.NotFound("Новая группа"));

        // Закрываем старое зачисление
        oldEnrollment.Status = EnrollmentStatus.Transferred;
        oldEnrollment.LeftAt = DateTime.UtcNow;
        oldEnrollment.TransferredToGroupId = request.NewGroupId;

        // Создаём новое
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

        _db.Add(newEnrollment);
        await _db.SaveChangesAsync(ct);

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
