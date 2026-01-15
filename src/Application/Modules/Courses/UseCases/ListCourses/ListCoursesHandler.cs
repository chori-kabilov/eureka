using Application.Abstractions;
using Application.Common;
using Application.Modules.Courses.Dtos;
using Application.Modules.Courses.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Courses.UseCases.ListCourses;

// Handler для получения списка курсов с фильтрацией и пагинацией
public class ListCoursesHandler(IDataContext db)
{
    public async Task<Result<PagedResult<CourseDto>>> HandleAsync(
        ListCoursesRequest request,
        CancellationToken ct = default)
    {
        var query = db.Courses.AsQueryable();

        // Фильтрация
        if (!string.IsNullOrWhiteSpace(request.Filter.Search))
        {
            var search = request.Filter.Search.ToLower();
            query = query.Where(c => c.Name.ToLower().Contains(search) || 
                                     (c.Description != null && c.Description.ToLower().Contains(search)));
        }

        if (request.Filter.Status.HasValue)
            query = query.Where(c => c.Status == request.Filter.Status.Value);

        if (request.Filter.StudentPaymentType.HasValue)
            query = query.Where(c => c.StudentPaymentType == request.Filter.StudentPaymentType.Value);

        // Подсчёт общего количества
        var totalCount = await query.CountAsync(ct);

        // Сортировка
        query = request.Filter.SortBy?.ToLower() switch
        {
            "name" => request.Filter.SortDesc ? query.OrderByDescending(c => c.Name) : query.OrderBy(c => c.Name),
            "createdat" => request.Filter.SortDesc ? query.OrderByDescending(c => c.CreatedAt) : query.OrderBy(c => c.CreatedAt),
            "status" => request.Filter.SortDesc ? query.OrderByDescending(c => c.Status) : query.OrderBy(c => c.Status),
            _ => query.OrderByDescending(c => c.CreatedAt)
        };

        // Пагинация
        var courses = await query
            .Skip(request.Pagination.Skip)
            .Take(request.Pagination.PageSize)
            .ToListAsync(ct);

        var result = new PagedResult<CourseDto>
        {
            Items = CourseMapper.ToDtoList(courses),
            Page = request.Pagination.Page,
            PageSize = request.Pagination.PageSize,
            TotalCount = totalCount
        };

        return Result<PagedResult<CourseDto>>.Success(result);
    }
}
