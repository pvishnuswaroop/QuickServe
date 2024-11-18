using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace QuickServe.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string? Name { get; set; }

        public string? Gender { get; set; }

        [Required(ErrorMessage = "Contact number is required.")]
        [Phone(ErrorMessage = "Invalid contact number format.")]
        [StringLength(15, ErrorMessage = "Contact number cannot exceed 15 characters.")]
        public string? ContactNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters.")]
        public string? Email { get; set; }  

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters.")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@$!%*?&]{6,}$", ErrorMessage = "Password must contain at least one letter and one number.")]
        public string? Password { get; set; }  // Ensure password is hashed before storing

        // Navigation properties
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();  // One-to-many with Order
        public virtual Cart? Cart { get; set; }  // One-to-one with Cart
        public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();  // One-to-many with Rating
    }
}
