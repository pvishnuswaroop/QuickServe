using System;
using System.ComponentModel.DataAnnotations;

namespace QuickServe.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemID { get; set; }

        [Required(ErrorMessage = "Order ID is required.")]
        public int OrderID { get; set; }  // Foreign Key

        [Required(ErrorMessage = "Menu ID is required.")]
        public int MenuID { get; set; }  // Foreign Key

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
        public int Quantity { get; set; }  // Ensure quantity is a positive integer

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }  // Ensure price is positive

        // Navigation properties
        public virtual Order? Order { get; set; }  // Many-to-one with Order
        public virtual Menu? Menu { get; set; }  // Many-to-one with Menu
    }
}
