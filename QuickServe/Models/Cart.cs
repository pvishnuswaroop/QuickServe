using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickServe.Models
{
    public class Cart
    {
        [Key]
        public int CartID { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        [ForeignKey("User")]
        public int UserID { get; set; }  // Foreign Key

        [Required(ErrorMessage = "Creation date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime CreationDate { get; set; } = DateTime.Now;  // Default value

        [Required]
        public bool IsActive { get; set; } = true;  // Indicates if the cart is currently active

        // Navigation properties
        public virtual User User { get; set; }  // Many-to-one with User (non-nullable)
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();  // One-to-many with CartItem
    }
}
