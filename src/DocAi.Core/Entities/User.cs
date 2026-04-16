using System.ComponentModel.DataAnnotations;

namespace DocAi.Core.Entities;

public class User : BaseEntity
{
    [Required][MaxLength(50)]
    public string FullName { get; set; } = string.Empty;

    [Required][MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    public string Role { get; set; } = "User";

    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
}
