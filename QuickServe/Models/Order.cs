using System;
using System.ComponentModel.DataAnnotations;

namespace QuickServe.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [Required]
        public int UserID { get; set; }  // Foreign Key

        [Required]
        public int RestaurantID { get; set; }  // Foreign Key

        [Required]
        public string? OrderStatus { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        // Navigation properties
        public virtual User? User { get; set; }  // Many-to-one with User
        public virtual Restaurant? Restaurant { get; set; }  // Many-to-one with Restaurant
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();  // One-to-many with OrderItem
        public virtual Payment? Payment { get; set; }  // One-to-one with Payment

    }
}
