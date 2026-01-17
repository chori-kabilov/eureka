using Application.Abstractions;
using Application.Common;
using Application.Modules.Teachers.Dtos;
using Application.Modules.Teachers.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Teachers.UseCases.ListTeachers;

// Handler получения списка учителей
public class ListTeachersHandler(IDataContext db)
{
    public async Task<Result<PagedResult<TeacherDto>>> HandleAsync(
        ListTeachersRequest request,
        CancellationToken ct = default)
    {
        var query = db.Teachers
            .Include(t => t.User)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            query = query.Where(t => 
                t.User!.Phone.ToLower().Contains(search) ||
                t.User.FullName.ToLower().Contains(search) ||
                t.Subjects.Any(s => s.ToLower().Contains(search)));
        }

        var totalCount = await query.CountAsync(ct);

        var skip = (request.Page - 1) * request.PageSize;
        var teachers = await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip(skip)
            .Take(request.PageSize)
            .ToListAsync(ct);

        var result = new PagedResult<TeacherDto>
        {
            Items = teachers.Select(TeacherMapper.ToDto).ToList(),
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };

        return Result<PagedResult<TeacherDto>>.Success(result);
    }
}
