using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Firstwebapp.Models;
public enum UserStatus
{
    Active,
    Inactive
}
public class UserModel
    {
        public Guid Id { get; set; }
       [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be exactly 10 digits")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits")]
        [Display(Name = "Phone Number")]
        
        public string Phone { get; set; }
        [Required]
        [Range(0,100)]
        public int Age { get; set; }
        
        public UserStatus Status { get; set; } = UserStatus.Active;
    }
