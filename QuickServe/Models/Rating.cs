using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickServe.Models
{
    public class Rating
    {
        [Key]
        public int RatingID { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        [ForeignKey("User")]
        public int UserID { get; set; }  // Foreign Key

        [Required(ErrorMessage = "Restaurant ID is required.")]
        [ForeignKey("Restaurant")]
        public int RestaurantID { get; set; }  // Foreign Key

        public int? OrderID { get; set; }  // Foreign Key, nullable

        [Required(ErrorMessage = "Rating score is required.")]
        [Range(1, 5, ErrorMessage = "Rating score must be between 1 and 5.")]
        public int RatingScore { get; set; }

        [StringLength(500, ErrorMessage = "Review text cannot exceed 500 characters.")]
        public string? ReviewText { get; set; }

        [Required(ErrorMessage = "Rating date is required.")]
        public DateTime RatingDate { get; set; }

        // Navigation properties
        public virtual User User { get; set; }  // Many-to-one with User
        public virtual Restaurant Restaurant { get; set; }  // Many-to-one with Restaurant
        public virtual Order? Order { get; set; }  // Many-to-one with Order (nullable)
    }
}
