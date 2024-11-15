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
        public string OrderStatus { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        // Constructor
        public Order(int userID, int restaurantID, string orderStatus, DateTime orderDate, decimal totalAmount)
        {
            UserID = userID;
            RestaurantID = restaurantID;
            OrderStatus = orderStatus;
            OrderDate = orderDate;
            TotalAmount = totalAmount;
        }
    }
}
