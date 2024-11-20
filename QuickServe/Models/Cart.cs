using System;
using System.ComponentModel.DataAnnotations;

namespace QuickServe.Models
{
    public class Cart
    {
        [Key]
        public int CartID { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public int UserID { get; set; }  // Foreign Key

        [Required(ErrorMessage = "Creation date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime CreationDate { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;  

        // Navigation property
        public virtual User User { get; set; }  // Many-to-one with User (non-nullable)
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();  // One-to-many with CartItem
    }
}
