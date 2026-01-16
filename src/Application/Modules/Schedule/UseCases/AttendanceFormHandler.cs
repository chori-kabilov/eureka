using Application.Abstractions;
using Application.Common;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Schedule.UseCases;

// DTO для формы посещаемости
public class AttendanceFormDto
{
    public LessonFormDto Lesson { get; set; } = null!;
    public List<StudentAttendanceFormDto> Students { get; set; } = new();
}

public class LessonFormDto
{
    public Guid Id { get; set; }
    public Guid GroupId { get; set; }
    public string GroupName { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}

public class StudentAttendanceFormDto
{
    public Guid? StudentId { get; set; }
    public Guid? ChildId { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsChild { get; set; }
    public int? Status { get; set; }
}

// Handler для получения формы посещаемости
public class GetAttendanceFormHandler
{
    private readonly IDataContext _db;

    public GetAttendanceFormHandler(IDataContext db) => _db = db;

    public async Task<Result<AttendanceFormDto>> HandleAsync(Guid lessonId, CancellationToken ct = default)
    {
        var lesson = await _db.Lessons
            .Include(l => l.Group)
            .FirstOrDefaultAsync(l => l.Id == lessonId, ct);

        if (lesson == null)
            return Result<AttendanceFormDto>.Failure(Error.NotFound("Занятие"));

        // Получаем студентов группы
        var enrollments = await _db.GroupEnrollments
            .Include(e => e.Student).ThenInclude(s => s!.User)
            .Include(e => e.Child)
            .Where(e => e.GroupId == lesson.GroupId && e.Status == EnrollmentStatus.Active)
            .ToListAsync(ct);

        // Получаем существующую посещаемость
        var attendances = await _db.Attendances
            .Where(a => a.LessonId == lessonId)
            .ToListAsync(ct);

        var students = enrollments.Select(e =>
        {
            var att = attendances.FirstOrDefault(a =>
                (e.StudentId.HasValue && a.StudentId == e.StudentId) ||
                (e.ChildId.HasValue && a.ChildId == e.ChildId));

            return new StudentAttendanceFormDto
            {
                StudentId = e.StudentId,
                ChildId = e.ChildId,
                Name = e.Student?.User?.FullName ?? e.Child?.FullName ?? "Неизвестно",
                IsChild = e.ChildId.HasValue,
                Status = att != null ? (int)att.Status : null
            };
        }).OrderBy(s => s.Name).ToList();

        return Result<AttendanceFormDto>.Success(new AttendanceFormDto
        {
            Lesson = new LessonFormDto
            {
                Id = lesson.Id,
                GroupId = lesson.GroupId,
                GroupName = lesson.Group?.Name ?? string.Empty,
                Date = lesson.Date,
                StartTime = lesson.StartTime,
                EndTime = lesson.EndTime
            },
            Students = students
        });
    }
}
