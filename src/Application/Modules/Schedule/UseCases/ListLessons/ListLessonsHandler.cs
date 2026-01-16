using Application.Abstractions;
using Application.Common;
using Application.Modules.Schedule.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Schedule.UseCases.ListLessons;

// Получить список занятий
public class ListLessonsHandler(IDataContext db)
{
    public async Task<Result<PagedResult<LessonDto>>> HandleAsync(ListLessonsRequest request, CancellationToken ct = default)
    {
        var query = db.Lessons
            .Include(l => l.Group)
            .Include(l => l.Teacher).ThenInclude(t => t.User)
            .Include(l => l.Room)
            .AsQueryable();

        if (request.GroupId.HasValue)
            query = query.Where(l => l.GroupId == request.GroupId);

        if (request.TeacherId.HasValue)
            query = query.Where(l => l.TeacherId == request.TeacherId);

        if (request.DateFrom.HasValue)
            query = query.Where(l => l.Date >= request.DateFrom.Value);

        if (request.DateTo.HasValue)
            query = query.Where(l => l.Date <= request.DateTo.Value);

        if (request.Status.HasValue)
            query = query.Where(l => l.Status == request.Status);

        var totalCount = await query.CountAsync(ct);
        var skip = (request.Page - 1) * request.PageSize;

        var lessons = await query
            .OrderBy(l => l.Date)
            .ThenBy(l => l.StartTime)
            .Skip(skip)
            .Take(request.PageSize)
            .ToListAsync(ct);

        var items = lessons.Select(l => new LessonDto
        {
            Id = l.Id,
            GroupId = l.GroupId,
            GroupName = l.Group?.Name ?? string.Empty,
            Date = l.Date,
            StartTime = l.StartTime,
            EndTime = l.EndTime,
            TeacherId = l.TeacherId,
            TeacherName = l.Teacher?.User?.FullName ?? string.Empty,
            RoomId = l.RoomId,
            RoomName = l.Room?.Name,
            Type = l.Type,
            Status = l.Status,
            Topic = l.Topic
        }).ToList();

        return Result<PagedResult<LessonDto>>.Success(new PagedResult<LessonDto>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount
        });
    }
}
