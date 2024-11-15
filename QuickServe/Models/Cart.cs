using System;
using System.ComponentModel.DataAnnotations;

namespace QuickServe.Models
{
    public class Cart
    {
        [Key]
        public int CartID { get; set; }

        [Required]
        public int UserID { get; set; }  // Foreign Key

        [Required]
        public DateTime CreationDate { get; set; }

        // Navigation property
        public virtual User? User { get; set; }  // Many-to-one with User
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();  // One-to-many with CartItem

    }

}
