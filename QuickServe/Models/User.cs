using System;
using System.ComponentModel.DataAnnotations;

namespace QuickServe.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Gender { get; set; }

        [Required]
        public string ContactNumber { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Password { get; set; }  // Hashed password

        // Navigation property
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();  // One-to-many with Order
        public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();  // One-to-many with Cart
        public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();  // One-to-many with Rating

        // Constructor
        public User(string name, string contactNumber, string email, string address, string password, string? gender = null)
        {
            Name = name;
            ContactNumber = contactNumber;
            Email = email;
            Address = address;
            Password = password;
            Gender = gender;
        }
    }
}
