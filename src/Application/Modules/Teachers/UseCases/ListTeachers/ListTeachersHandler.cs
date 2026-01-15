using Application.Abstractions;
using Application.Common;
using Application.Modules.Teachers.Dtos;
using Application.Modules.Teachers.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Teachers.UseCases.ListTeachers;

// Handler получения списка учителей
public class ListTeachersHandler
{
    private readonly IDataContext _db;

    public ListTeachersHandler(IDataContext db)
    {
        _db = db;
    }

    public async Task<Result<PagedResult<TeacherDto>>> HandleAsync(
        ListTeachersRequest request,
        CancellationToken ct = default)
    {
        var query = _db.Teachers
            .Include(t => t.User)
            .AsQueryable();

        // Фильтр по поиску
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            query = query.Where(t => 
                t.User!.Phone.ToLower().Contains(search) ||
                t.User.FullName.ToLower().Contains(search) ||
                (t.Specialization != null && t.Specialization.ToLower().Contains(search)));
        }

        // Подсчёт
        var totalCount = await query.CountAsync(ct);

        // Пагинация
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
