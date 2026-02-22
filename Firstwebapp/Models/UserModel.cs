using System.ComponentModel.DataAnnotations;

namespace Firstwebapp.Models;

    public class UserModel
    {
       [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Range(0,100)]
        public int Age { get; set; }
    }
