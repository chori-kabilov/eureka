using Domain.Common;
using Domain.Users;

namespace Domain.Teachers;

// Профиль учителя (расширение User)
public class Teacher : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    public TeacherStatus Status { get; set; } = TeacherStatus.Active;
    public List<string> Subjects { get; set; } = new();
    public TeacherPaymentType PaymentType { get; set; }
    public decimal? HourlyRate { get; set; }
    public string? Bio { get; set; }
    public DateTime? HiredAt { get; set; }
}

