using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace QuickServe.Models
{
    public enum UserRole
    {
        Admin,
        Customer,
        RestaurantOwner,
        DeliveryPerson
    }

    public enum Gender
    {
        Male,
        Female,
        Other,
        PreferNotToSay
    }

    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        public Gender? Gender { get; set; }

        [Required(ErrorMessage = "Contact number is required.")]
        [Phone(ErrorMessage = "Invalid contact number format.")]
        [StringLength(15, ErrorMessage = "Contact number cannot exceed 15 characters.")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters.")]

        public string Email { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters.")]
        public string Address { get; set; }

        // Only store the hashed password
        [Required]
        public string PasswordHash { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual Cart Cart { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

        public DateTime? DateOfBirth { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastLogin { get; set; }

        [Required]
        public UserRole Role { get; set; }

        public List<string> Roles { get; set; }

        // Password validation using hashed password comparison
        public bool ValidatePassword(string inputPassword)
        {
            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(this, PasswordHash, inputPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
