using System.ComponentModel.DataAnnotations;
using Firstwebapp.Models;

namespace Firstwebapp.ViewModels
{
    public class UserProfileViewModel
    {
        public Guid Id { get; set; }
        
        [Display(Name = "Full Name")]
        public string Name { get; set; }
        
        public string Email { get; set; }
        
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }
        
        public int Age { get; set; }
        
        [Display(Name = "Account Status")]
        public UserStatus Status { get; set; }
        
        [Display(Name = "Member Since")]
        public DateTime CreatedAt { get; set; }
        
        // Additional profile info (calculated/from other tables)
        [Display(Name = "Total Jobs")]
        public int TotalJobs { get; set; }
        
        [Display(Name = "Active Jobs")]
        public int ActiveJobs { get; set; }
    }
}