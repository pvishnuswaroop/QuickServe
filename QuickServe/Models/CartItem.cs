using System;
using System.ComponentModel.DataAnnotations;

namespace QuickServe.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemID { get; set; }

        [Required]
        public int CartID { get; set; }  // Foreign Key

        [Required]
        public int MenuID { get; set; }  // Foreign Key

        [Required]
        public int Quantity { get; set; }

        // Navigation properties
        public virtual Cart? Cart { get; set; }  // Many-to-one with Cart
        public virtual Menu? Menu { get; set; }  // Many-to-one with Menu

        // Constructor
        public CartItem(int cartID, int menuID, int quantity)
        {
            CartID = cartID;
            MenuID = menuID;
            Quantity = quantity;
        }
    }

}
