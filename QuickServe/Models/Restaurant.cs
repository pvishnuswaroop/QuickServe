﻿using System;
using System.ComponentModel.DataAnnotations;

namespace QuickServe.Models
{
    public class Restaurant
    {
        [Key]
        public int RestaurantID { get; set; }

        [Required(ErrorMessage = "Restaurant name is required.")]
        [StringLength(100, ErrorMessage = "Restaurant name cannot exceed 100 characters.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(200, ErrorMessage = "Location cannot exceed 200 characters.")]
        public string? Location { get; set; }

        [Required(ErrorMessage = "Contact number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(15, ErrorMessage = "Contact number cannot exceed 15 characters.")]
        public string? ContactNumber { get; set; }

        // Navigation property
        public virtual ICollection<Menu> Menus { get; set; } = new List<Menu>();  // One-to-many with Menu
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();  // One-to-many with Order
        public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();  // One-to-many with Rating
    }
}
