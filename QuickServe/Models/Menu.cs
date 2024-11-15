using System;
using System.ComponentModel.DataAnnotations;

namespace QuickServe.Models
{
    public class Menu
    {
        [Key]
        public int MenuID { get; set; }

        [Required]
        public int RestaurantID { get; set; }  // Foreign Key

        [Required]
        public string? ItemName { get; set; }

        public string? Description { get; set; }

        [Required]
        public string? Category { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string? AvailabilityTime { get; set; }

        public string? DietaryInfo { get; set; }

        [Required]
        public string? Status { get; set; }

        // Navigation property
        public virtual Restaurant? Restaurant { get; set; }  // Many-to-one with Restaurant
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();  // One-to-many with OrderItem
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();  // One-to-many with CartItem

    }

}
