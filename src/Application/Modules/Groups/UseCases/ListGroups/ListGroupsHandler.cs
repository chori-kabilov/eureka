using Application.Abstractions;
using Application.Common;
using Application.Modules.Groups.Dtos;
using Application.Modules.Groups.Mapping;
using Domain.Groups;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Groups.UseCases.ListGroups;

// Получить список групп
public class ListGroupsHandler(IDataContext db)
{
    public async Task<Result<PagedResult<GroupDto>>> HandleAsync(ListGroupsRequest request, CancellationToken ct = default)
    {
        var query = db.Groups
            .Include(g => g.Course)
            .Include(g => g.ResponsibleTeacher).ThenInclude(t => t.User)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            query = query.Where(g => g.Name.ToLower().Contains(search) || 
                                     (g.Code != null && g.Code.ToLower().Contains(search)));
        }

        if (request.CourseId.HasValue)
            query = query.Where(g => g.CourseId == request.CourseId);

        if (request.TeacherId.HasValue)
            query = query.Where(g => g.ResponsibleTeacherId == request.TeacherId || 
                                     g.DefaultTeacherId == request.TeacherId);

        if (request.Status.HasValue)
            query = query.Where(g => g.Status == request.Status);

        var totalCount = await query.CountAsync(ct);
        var skip = (request.Page - 1) * request.PageSize;

        var groups = await query
            .OrderByDescending(g => g.CreatedAt)
            .Skip(skip)
            .Take(request.PageSize)
            .ToListAsync(ct);

        var groupIds = groups.Select(g => g.Id).ToList();
        var studentCounts = await db.GroupEnrollments
            .Where(e => groupIds.Contains(e.GroupId) && e.Status == EnrollmentStatus.Active)
            .GroupBy(e => e.GroupId)
            .Select(g => new { GroupId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.GroupId, x => x.Count, ct);

        var items = groups.Select(g => GroupMapper.ToDto(g, studentCounts.GetValueOrDefault(g.Id, 0))).ToList();

        return Result<PagedResult<GroupDto>>.Success(new PagedResult<GroupDto>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount
        });
    }
}
