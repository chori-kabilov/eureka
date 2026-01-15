using Domain.Courses;

namespace Application.Modules.Courses.Dtos;

// Фильтры для списка курсов
public class CourseFilterDto
{
    public string? Search { get; set; }
    public CourseStatus? Status { get; set; }
    public StudentPaymentType? StudentPaymentType { get; set; }
    public string? SortBy { get; set; }
    public bool SortDesc { get; set; }
}
