using System;
using System.ComponentModel.DataAnnotations;

namespace QuickServe.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemID { get; set; }

        [Required(ErrorMessage = "Cart ID is required.")]
        public int CartID { get; set; }  // Foreign Key

        [Required(ErrorMessage = "Menu ID is required.")]
        public int MenuID { get; set; }  // Foreign Key

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
        public int Quantity { get; set; }

        // Navigation properties
        public virtual Cart? Cart { get; set; }  // Many-to-one with Cart
        public virtual Menu? Menu { get; set; }  // Many-to-one with Menu
    }
}
