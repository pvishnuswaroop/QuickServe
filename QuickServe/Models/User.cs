using System;
using System.ComponentModel.DataAnnotations;

namespace QuickServe.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }

        public string? Gender { get; set; }

        [Required(ErrorMessage = "Contact number is required.")]
        public string? ContactNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set; }  // Hashed password

        // Navigation property
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();  // One-to-many with Order
        public virtual Cart? Cart { get; set; }  // One-to-one with Cart
        public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();  // One-to-many with Rating
    }
}
