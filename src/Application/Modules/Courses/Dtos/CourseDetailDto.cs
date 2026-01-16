namespace Application.Modules.Courses.Dtos;

// DTO для детальной информации о курсе
public class CourseDetailDto : CourseDto
{
    public Guid? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
}
