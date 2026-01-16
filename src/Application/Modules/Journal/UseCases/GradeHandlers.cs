using Application.Abstractions;
using Application.Common;
using Application.Modules.Journal.Dtos;
using Domain.Journal;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Journal.UseCases;

// Получить оценки занятия
public class GetLessonGradesHandler
{
    private readonly IDataContext _db;

    public GetLessonGradesHandler(IDataContext db) => _db = db;

    public async Task<Result<List<GradeDto>>> HandleAsync(Guid lessonId, CancellationToken ct = default)
    {
        var grades = await _db.Grades
            .Include(g => g.Student).ThenInclude(s => s!.User)
            .Include(g => g.Child)
            .Include(g => g.GradingSystem)
            .Include(g => g.GradedBy)
            .Where(g => g.LessonId == lessonId)
            .ToListAsync(ct);

        var items = grades.Select(g => new GradeDto
        {
            Id = g.Id,
            LessonId = g.LessonId,
            StudentId = g.StudentId,
            ChildId = g.ChildId,
            StudentName = g.Student?.User?.FullName ?? g.Child?.FullName ?? string.Empty,
            Score = g.Score,
            Letter = g.Letter,
            Weight = g.Weight,
            Comment = g.Comment,
            GradingSystemId = g.GradingSystemId,
            GradingSystemName = g.GradingSystem?.Name ?? string.Empty,
            GradedAt = g.GradedAt,
            GradedByName = g.GradedBy?.FullName ?? string.Empty
        }).ToList();

        return Result<List<GradeDto>>.Success(items);
    }
}

// Поставить оценку
public class SetGradeRequest
{
    public Guid LessonId { get; set; }
    public Guid? StudentId { get; set; }
    public Guid? ChildId { get; set; }
    public decimal Score { get; set; }
    public decimal Weight { get; set; } = 1;
    public string? Comment { get; set; }
}

public class SetGradeHandler
{
    private readonly IDataContext _db;
    private readonly ICurrentUser _currentUser;

    public SetGradeHandler(IDataContext db, ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<Result<GradeDto>> HandleAsync(SetGradeRequest request, CancellationToken ct = default)
    {
        // Получаем занятие и группу для системы оценок
        var lesson = await _db.Lessons
            .Include(l => l.Group)
            .FirstOrDefaultAsync(l => l.Id == request.LessonId, ct);

        if (lesson == null)
            return Result<GradeDto>.Failure(Error.NotFound("Занятие"));

        // Система оценок из группы или по умолчанию
        var gradingSystemId = lesson.Group?.GradingSystemId;
        if (!gradingSystemId.HasValue)
        {
            var defaultSystem = await _db.GradingSystems.FirstOrDefaultAsync(g => g.IsDefault, ct);
            gradingSystemId = defaultSystem?.Id ?? Guid.Empty;
        }

        if (gradingSystemId == Guid.Empty)
            return Result<GradeDto>.Failure(Error.Validation("Не настроена система оценок"));

        // Ищем существующую оценку
        var existing = await _db.Grades
            .FirstOrDefaultAsync(g => g.LessonId == request.LessonId &&
                ((request.StudentId.HasValue && g.StudentId == request.StudentId) ||
                 (request.ChildId.HasValue && g.ChildId == request.ChildId)), ct);

        if (existing != null)
        {
            existing.Score = request.Score;
            existing.Weight = request.Weight;
            existing.Comment = request.Comment;
            existing.GradedAt = DateTime.UtcNow;
            existing.GradedById = _currentUser.UserId ?? Guid.Empty;

            await _db.SaveChangesAsync(ct);

            return Result<GradeDto>.Success(new GradeDto
            {
                Id = existing.Id,
                LessonId = existing.LessonId,
                StudentId = existing.StudentId,
                ChildId = existing.ChildId,
                Score = existing.Score,
                Weight = existing.Weight,
                Comment = existing.Comment,
                GradedAt = existing.GradedAt
            });
        }

        var grade = new Grade
        {
            Id = Guid.NewGuid(),
            LessonId = request.LessonId,
            StudentId = request.StudentId,
            ChildId = request.ChildId,
            GradingSystemId = gradingSystemId.Value,
            Score = request.Score,
            Weight = request.Weight,
            Comment = request.Comment,
            GradedById = _currentUser.UserId ?? Guid.Empty,
            GradedAt = DateTime.UtcNow
        };

        _db.Add(grade);
        await _db.SaveChangesAsync(ct);

        return Result<GradeDto>.Success(new GradeDto
        {
            Id = grade.Id,
            LessonId = grade.LessonId,
            StudentId = grade.StudentId,
            ChildId = grade.ChildId,
            Score = grade.Score,
            Weight = grade.Weight,
            Comment = grade.Comment,
            GradingSystemId = grade.GradingSystemId,
            GradedAt = grade.GradedAt
        });
    }
}

// Получить журнал группы
public class GetGroupJournalRequest
{
    public Guid GroupId { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
}

public class GetGroupJournalHandler
{
    private readonly IDataContext _db;

    public GetGroupJournalHandler(IDataContext db) => _db = db;

    public async Task<Result<List<JournalRowDto>>> HandleAsync(GetGroupJournalRequest request, CancellationToken ct = default)
    {
        // Получаем занятия группы
        var lessonsQuery = _db.Lessons
            .Where(l => l.GroupId == request.GroupId && l.Status != Domain.Enums.LessonStatus.Cancelled);

        if (request.DateFrom.HasValue)
            lessonsQuery = lessonsQuery.Where(l => l.Date >= request.DateFrom.Value);
        if (request.DateTo.HasValue)
            lessonsQuery = lessonsQuery.Where(l => l.Date <= request.DateTo.Value);

        var lessons = await lessonsQuery
            .OrderBy(l => l.Date)
            .ThenBy(l => l.StartTime)
            .ToListAsync(ct);

        if (!lessons.Any())
            return Result<List<JournalRowDto>>.Success(new List<JournalRowDto>());

        var lessonIds = lessons.Select(l => l.Id).ToList();

        // Получаем студентов группы
        var enrollments = await _db.GroupEnrollments
            .Include(e => e.Student).ThenInclude(s => s!.User)
            .Include(e => e.Child)
            .Where(e => e.GroupId == request.GroupId && e.Status == Domain.Enums.EnrollmentStatus.Active)
            .ToListAsync(ct);

        // Получаем посещаемость и оценки
        var attendances = await _db.Attendances
            .Where(a => lessonIds.Contains(a.LessonId))
            .ToListAsync(ct);

        var grades = await _db.Grades
            .Where(g => lessonIds.Contains(g.LessonId))
            .ToListAsync(ct);

        // Формируем журнал
        var result = new List<JournalRowDto>();

        foreach (var enrollment in enrollments)
        {
            var row = new JournalRowDto
            {
                StudentId = enrollment.StudentId,
                ChildId = enrollment.ChildId,
                StudentName = enrollment.Student?.User?.FullName ?? enrollment.Child?.FullName ?? "Неизвестно",
                IsChild = enrollment.ChildId.HasValue,
                TotalLessons = lessons.Count
            };

            int attended = 0;
            decimal gradeSum = 0;
            decimal weightSum = 0;

            foreach (var lesson in lessons)
            {
                var att = attendances.FirstOrDefault(a => a.LessonId == lesson.Id &&
                    ((enrollment.StudentId.HasValue && a.StudentId == enrollment.StudentId) ||
                     (enrollment.ChildId.HasValue && a.ChildId == enrollment.ChildId)));

                var grade = grades.FirstOrDefault(g => g.LessonId == lesson.Id &&
                    ((enrollment.StudentId.HasValue && g.StudentId == enrollment.StudentId) ||
                     (enrollment.ChildId.HasValue && g.ChildId == enrollment.ChildId)));

                row.Cells.Add(new JournalCellDto
                {
                    LessonId = lesson.Id,
                    Date = lesson.Date,
                    LessonType = lesson.Type,
                    AttendanceStatus = att?.Status,
                    Grade = grade?.Score,
                    GradeLetter = grade?.Letter
                });

                if (att?.Status == Domain.Enums.AttendanceStatus.Present || 
                    att?.Status == Domain.Enums.AttendanceStatus.Late)
                    attended++;

                if (grade != null)
                {
                    gradeSum += grade.Score * grade.Weight;
                    weightSum += grade.Weight;
                }
            }

            row.AttendedLessons = attended;
            row.AttendancePercent = lessons.Count > 0 ? (int)(attended * 100.0 / lessons.Count) : 0;
            row.AverageGrade = weightSum > 0 ? Math.Round(gradeSum / weightSum, 2) : null;

            result.Add(row);
        }

        return Result<List<JournalRowDto>>.Success(result.OrderBy(r => r.StudentName).ToList());
    }
}
