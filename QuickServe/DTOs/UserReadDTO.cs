using QuickServe.Models;
using System.ComponentModel.DataAnnotations;

namespace QuickServe.DTOs
{
    public class UserCreateDTO
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        public string Gender { get; set; }

        [Required]
        [Phone(ErrorMessage = "Invalid contact number format.")]
        public string ContactNumber { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public UserRole Role { get; set; }
    }
}
