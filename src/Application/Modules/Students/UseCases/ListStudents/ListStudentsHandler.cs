using Application.Abstractions;
using Application.Common;
using Application.Modules.Students.Dtos;
using Application.Modules.Students.Mapping;
using Domain.Students;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Students.UseCases.ListStudents;

// Handler получения списка студентов
public class ListStudentsHandler(IDataContext db)
{
    public async Task<Result<PagedResult<StudentDto>>> HandleAsync(
        ListStudentsRequest request,
        CancellationToken ct = default)
    {
        var query = db.Students
            .Include(s => s.User)
            .AsQueryable();

        // Фильтр по поиску
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            query = query.Where(s => 
                s.User!.Phone.ToLower().Contains(search) ||
                s.User.FullName.ToLower().Contains(search));
        }

        // Фильтр по статусу
        if (request.Status.HasValue)
        {
            var status = (StudentStatus)request.Status.Value;
            query = query.Where(s => s.Status == status);
        }

        // Подсчёт
        var totalCount = await query.CountAsync(ct);

        // Пагинация
        var skip = (request.Page - 1) * request.PageSize;
        var students = await query
            .OrderByDescending(s => s.CreatedAt)
            .Skip(skip)
            .Take(request.PageSize)
            .ToListAsync(ct);

        var result = new PagedResult<StudentDto>
        {
            Items = students.Select(StudentMapper.ToDto).ToList(),
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };

        return Result<PagedResult<StudentDto>>.Success(result);
    }
}
