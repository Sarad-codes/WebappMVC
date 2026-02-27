using System.ComponentModel.DataAnnotations;

namespace Firstwebapp.Models;

public enum UserStatus
{
    Active,
    Inactive
}

public class UserModel
{
    public Guid Id { get; set; }

    [Required] public string Name { get; set; }

    [Required] [EmailAddress] public string Email { get; set; }

    [Required]
    [StringLength(10, MinimumLength = 10)]
    public string Phone { get; set; }

    [Required] [Range(0, 100)] public int Age { get; set; }

    public UserStatus Status { get; set; } = UserStatus.Active;

    // Add these for authentication
    public string PasswordHash { get; set; } // Store hashed password
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? LastLoginAt { get; set; }

    // ... existing properties ..
    // For password reset
    public string? PasswordResetToken { get; set; }
    public DateTime? ResetTokenExpires { get; set; }
}