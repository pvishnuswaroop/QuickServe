﻿using System;
using System.ComponentModel.DataAnnotations;

namespace QuickServe.Models
{
    public class Menu
    {
        [Key]
        public int MenuID { get; set; }

        [Required(ErrorMessage = "Restaurant ID is required.")]
        public int RestaurantID { get; set; }  // Foreign Key

        [Required(ErrorMessage = "Item name is required.")]
        [StringLength(100, ErrorMessage = "Item name cannot exceed 100 characters.")]
        public string? ItemName { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        [StringLength(50, ErrorMessage = "Category cannot exceed 50 characters.")]
        public string? Category { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [StringLength(100, ErrorMessage = "Availability time cannot exceed 100 characters.")]
        public string? AvailabilityTime { get; set; }

        [StringLength(200, ErrorMessage = "Dietary info cannot exceed 200 characters.")]
        public string? DietaryInfo { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters.")]
        public string? Status { get; set; }

        // Navigation property
        public virtual Restaurant? Restaurant { get; set; }  // Many-to-one with Restaurant
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();  // One-to-many with OrderItem
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();  // One-to-many with CartItem
    }
}
