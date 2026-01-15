using System.ComponentModel.DataAnnotations;

namespace WebApi.Contracts.Users;

// API контракт для изменения Admin статуса
public class UpdateAdminApiRequest
{
    [Required]
    public bool IsAdmin { get; set; }
}
