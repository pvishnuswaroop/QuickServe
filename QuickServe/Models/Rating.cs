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

        // Constructor
        public Rating(int userID, int restaurantID, int ratingScore, DateTime ratingDate, int? orderID = null, string? reviewText = null)
        {
            UserID = userID;
            RestaurantID = restaurantID;
            RatingScore = ratingScore;
            RatingDate = ratingDate;
            OrderID = orderID;
            ReviewText = reviewText;
        }
    }
}
