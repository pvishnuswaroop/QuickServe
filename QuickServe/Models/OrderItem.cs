using System;
using System.ComponentModel.DataAnnotations;

namespace QuickServe.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemID { get; set; }

        [Required]
        public int OrderID { get; set; }  // Foreign Key

        [Required]
        public int MenuID { get; set; }  // Foreign Key

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        // Constructor
        public OrderItem(int orderID, int menuID, int quantity, decimal price)
        {
            OrderID = orderID;
            MenuID = menuID;
            Quantity = quantity;
            Price = price;
        }
    }
}
