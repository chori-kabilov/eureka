using Domain.Common;
using Domain.Users;

namespace Domain.Teachers;

// Профиль учителя (расширение User)
public class Teacher : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    public string? Specialization { get; set; }
    public TeacherPaymentType PaymentType { get; set; }
    public decimal? HourlyRate { get; set; }
    public string? Bio { get; set; }
}
