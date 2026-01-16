using Application.Abstractions;
using Application.Common;
using Application.Modules.Journal.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Journal.UseCases.GetLessonGrades;

// Получить оценки занятия
public class GetLessonGradesHandler(IDataContext db)
{
    public async Task<Result<List<GradeDto>>> HandleAsync(Guid lessonId, CancellationToken ct = default)
    {
        var grades = await db.Grades
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
