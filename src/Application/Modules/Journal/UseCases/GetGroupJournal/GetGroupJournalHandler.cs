using Application.Abstractions;
using Application.Common;
using Application.Modules.Journal.Dtos;
using Domain.Groups;
using Domain.Journal;
using Domain.Schedule;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Journal.UseCases.GetGroupJournal;

// Получить журнал группы
public class GetGroupJournalHandler(IDataContext db)
{
    public async Task<Result<List<JournalRowDto>>> HandleAsync(GetGroupJournalRequest request, CancellationToken ct = default)
    {
        var lessonsQuery = db.Lessons
            .Where(l => l.GroupId == request.GroupId && l.Status != LessonStatus.Cancelled);

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

        var enrollments = await db.GroupEnrollments
            .Include(e => e.Student).ThenInclude(s => s!.User)
            .Include(e => e.Child)
            .Where(e => e.GroupId == request.GroupId && e.Status == EnrollmentStatus.Active)
            .ToListAsync(ct);

        var attendances = await db.Attendances
            .Where(a => lessonIds.Contains(a.LessonId))
            .ToListAsync(ct);

        var grades = await db.Grades
            .Where(g => lessonIds.Contains(g.LessonId))
            .ToListAsync(ct);

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

                if (att?.Status == AttendanceStatus.Present || 
                    att?.Status == AttendanceStatus.Late)
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
