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

        // Navigation properties
        public virtual Order? Order { get; set; }  // Many-to-one with Order
        public virtual Menu? Menu { get; set; }  // Many-to-one with Menu
    }

}
