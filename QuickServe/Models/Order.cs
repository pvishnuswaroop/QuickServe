using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickServe.Models
{
    public enum OrderStatus
    {
        Pending,
        Confirmed,
        InProgress,
        Completed,
        Canceled
    }

    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        [ForeignKey("User")]
        public int UserID { get; set; }  // Foreign Key

        [Required(ErrorMessage = "Restaurant ID is required.")]
        [ForeignKey("Restaurant")]
        public int RestaurantID { get; set; }  // Foreign Key

        [Required(ErrorMessage = "Order status is required.")]
        public OrderStatus OrderStatus { get; set; }  // Enum for order status

        [Required(ErrorMessage = "Order date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Total amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than zero.")]
        public decimal TotalAmount { get; set; }

        // Navigation properties
        public virtual User User { get; set; }  // Many-to-one with User
        public virtual Restaurant Restaurant { get; set; }  // Many-to-one with Restaurant
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();  // One-to-many with OrderItem
        public virtual Payment Payment { get; set; }  // One-to-one with Payment
    }
}
