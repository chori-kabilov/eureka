namespace RazorWebApp.Models.Groups;

// ViewModel группы
public class GroupViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    
    public Guid CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    
    public Guid ResponsibleTeacherId { get; set; }
    public string ResponsibleTeacherName { get; set; } = string.Empty;
    
    public Guid? DefaultTeacherId { get; set; }
    public string? DefaultTeacherName { get; set; }
    
    public Guid? DefaultRoomId { get; set; }
    public string? DefaultRoomName { get; set; }
    
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    public int MaxStudents { get; set; }
    public int CurrentStudents { get; set; }
    
    public int Status { get; set; }
    public string? Notes { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public string StatusName => Status switch
    {
        0 => "Черновик",
        1 => "Активная",
        2 => "Завершена",
        3 => "Отменена",
        _ => "Неизвестно"
    };
    
    public string StatusBadgeClass => Status switch
    {
        0 => "bg-secondary",
        1 => "bg-success",
        2 => "bg-primary",
        3 => "bg-danger",
        _ => "bg-secondary"
    };
    
    public string StudentsInfo => $"{CurrentStudents}/{MaxStudents}";
}

// ViewModel для студента группы
public class EnrollmentViewModel
{
    public Guid Id { get; set; }
    public Guid GroupId { get; set; }
    
    public Guid? StudentId { get; set; }
    public string? StudentName { get; set; }
    public string? StudentPhone { get; set; }
    
    public Guid? ChildId { get; set; }
    public string? ChildName { get; set; }
    public string? ParentName { get; set; }
    
    public DateTime EnrolledAt { get; set; }
    public int Status { get; set; }
    
    public string DisplayName => StudentName ?? ChildName ?? "Неизвестно";
    public bool IsChild => ChildId.HasValue;
}
