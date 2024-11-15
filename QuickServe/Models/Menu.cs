﻿using System;
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
        public string ItemName { get; set; }

        public string? Description { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string? AvailabilityTime { get; set; }

        public string? DietaryInfo { get; set; }

        [Required]
        public string Status { get; set; }

        // Constructor
        public Menu(int restaurantID, string itemName, string category, decimal price, string status, string? description = null, string? availabilityTime = null, string? dietaryInfo = null)
        {
            RestaurantID = restaurantID;
            ItemName = itemName;
            Category = category;
            Price = price;
            Status = status;
            Description = description;
            AvailabilityTime = availabilityTime;
            DietaryInfo = dietaryInfo;
        }
    }
}
