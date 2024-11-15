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

        // Constructor
        public CartItem(int cartID, int menuID, int quantity)
        {
            CartID = cartID;
            MenuID = menuID;
            Quantity = quantity;
        }
    }
}
