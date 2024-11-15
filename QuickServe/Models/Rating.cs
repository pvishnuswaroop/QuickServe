using System;
using System.ComponentModel.DataAnnotations;

namespace QuickServe.Models
{
    public class Rating
    {
        [Key]
        public int RatingID { get; set; }

        [Required]
        public int UserID { get; set; }  // Foreign Key

        [Required]
        public int RestaurantID { get; set; }  // Foreign Key

        public int? OrderID { get; set; }  // Foreign Key, nullable

        [Required]
        public int RatingScore { get; set; }

        public string? ReviewText { get; set; }

        [Required]
        public DateTime RatingDate { get; set; }

        // Navigation properties
        public virtual User? User { get; set; }  // Many-to-one with User
        public virtual Restaurant? Restaurant { get; set; }  // Many-to-one with Restaurant
        public virtual Order? Order { get; set; }  // Many-to-one with Order (nullable)

    }

}
