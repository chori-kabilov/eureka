using Domain.Common;
using Domain.Users;

namespace Domain.Parents;

// Профиль родителя (расширение User)
public class Parent : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    // Дети
    public ICollection<Child> Children { get; set; } = new List<Child>();
}
