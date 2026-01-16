using Application.Abstractions;
using Application.Common;
using Application.Modules.Journal.Dtos;
using Domain.Journal;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Journal.UseCases.SetGrade;

// Поставить оценку
public class SetGradeHandler(IDataContext db, ICurrentUser currentUser)
{
    public async Task<Result<GradeDto>> HandleAsync(SetGradeRequest request, CancellationToken ct = default)
    {
        var lesson = await db.Lessons
            .Include(l => l.Group)
            .FirstOrDefaultAsync(l => l.Id == request.LessonId, ct);

        if (lesson == null)
            return Result<GradeDto>.Failure(Error.NotFound("Занятие"));

        var gradingSystemId = lesson.Group?.GradingSystemId;
        if (!gradingSystemId.HasValue)
        {
            var defaultSystem = await db.GradingSystems.FirstOrDefaultAsync(g => g.IsDefault, ct);
            gradingSystemId = defaultSystem?.Id ?? Guid.Empty;
        }

        if (gradingSystemId == Guid.Empty)
            return Result<GradeDto>.Failure(Error.Validation("Не настроена система оценок"));

        var existing = await db.Grades
            .FirstOrDefaultAsync(g => g.LessonId == request.LessonId &&
                ((request.StudentId.HasValue && g.StudentId == request.StudentId) ||
                 (request.ChildId.HasValue && g.ChildId == request.ChildId)), ct);

        if (existing != null)
        {
            existing.Score = request.Score;
            existing.Weight = request.Weight;
            existing.Comment = request.Comment;
            existing.GradedAt = DateTime.UtcNow;
            existing.GradedById = currentUser.UserId ?? Guid.Empty;

            await db.SaveChangesAsync(ct);

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
            GradedById = currentUser.UserId ?? Guid.Empty,
            GradedAt = DateTime.UtcNow
        };

        db.Add(grade);
        await db.SaveChangesAsync(ct);

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
