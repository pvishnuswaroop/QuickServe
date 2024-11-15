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
