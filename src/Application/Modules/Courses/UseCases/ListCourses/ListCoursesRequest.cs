using Application.Common;
using Application.Modules.Courses.Dtos;

namespace Application.Modules.Courses.UseCases.ListCourses;

// Запрос на получение списка курсов
public class ListCoursesRequest
{
    public PaginationParams Pagination { get; set; } = new();
    public CourseFilterDto Filter { get; set; } = new();
}
