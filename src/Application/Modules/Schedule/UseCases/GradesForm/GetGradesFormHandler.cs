using Application.Abstractions;
using Application.Common;
using Domain.Groups;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Schedule.UseCases.GradesForm;

// DTO для формы оценок
public class GradesFormDto
{
    public LessonFormDto Lesson { get; set; } = null!;
    public List<StudentGradeFormDto> Students { get; set; } = new();
    public string? GradingSystemName { get; set; }
    public decimal MinScore { get; set; }
    public decimal MaxScore { get; set; }
    public decimal PassingScore { get; set; }
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

public class StudentGradeFormDto
{
    public Guid? StudentId { get; set; }
    public Guid? ChildId { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsChild { get; set; }
    public decimal? Score { get; set; }
    public decimal Weight { get; set; } = 1;
    public string? Comment { get; set; }
}

// Handler для получения формы оценок
public class GetGradesFormHandler(IDataContext db)
{
    public async Task<Result<GradesFormDto>> HandleAsync(Guid lessonId, CancellationToken ct = default)
    {
        var lesson = await db.Lessons
            .Include(l => l.Group).ThenInclude(g => g!.GradingSystem)
            .FirstOrDefaultAsync(l => l.Id == lessonId, ct);

        if (lesson == null)
            return Result<GradesFormDto>.Failure(Error.NotFound("Занятие"));

        var gradingSystem = lesson.Group?.GradingSystem;
        if (gradingSystem == null)
        {
            gradingSystem = await db.GradingSystems.FirstOrDefaultAsync(g => g.IsDefault, ct);
        }

        var enrollments = await db.GroupEnrollments
            .Include(e => e.Student).ThenInclude(s => s!.User)
            .Include(e => e.Child)
            .Where(e => e.GroupId == lesson.GroupId && e.Status == EnrollmentStatus.Active)
            .ToListAsync(ct);

        var grades = await db.Grades
            .Where(g => g.LessonId == lessonId)
            .ToListAsync(ct);

        var students = enrollments.Select(e =>
        {
            var grade = grades.FirstOrDefault(g =>
                (e.StudentId.HasValue && g.StudentId == e.StudentId) ||
                (e.ChildId.HasValue && g.ChildId == e.ChildId));

            return new StudentGradeFormDto
            {
                StudentId = e.StudentId,
                ChildId = e.ChildId,
                Name = e.Student?.User?.FullName ?? e.Child?.FullName ?? "Неизвестно",
                IsChild = e.ChildId.HasValue,
                Score = grade?.Score,
                Weight = grade?.Weight ?? 1,
                Comment = grade?.Comment
            };
        }).OrderBy(s => s.Name).ToList();

        return Result<GradesFormDto>.Success(new GradesFormDto
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
            Students = students,
            GradingSystemName = gradingSystem?.Name,
            MinScore = gradingSystem?.MinScore ?? 1,
            MaxScore = gradingSystem?.MaxScore ?? 5,
            PassingScore = gradingSystem?.PassingScore ?? 3
        });
    }
}
