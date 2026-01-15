namespace Application.Modules.Courses.Dtos;

public class CourseDetailDto : CourseDto
{
    public Guid? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
}