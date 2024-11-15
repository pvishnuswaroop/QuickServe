using System;
using System.ComponentModel.DataAnnotations;

namespace QuickServe.Models
{
    public class Restaurant
    {
        [Key]
        public int RestaurantID { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Location { get; set; }

        [Required]
        public string? ContactNumber { get; set; }

        // Navigation property
        public virtual ICollection<Menu> Menus { get; set; } = new List<Menu>();  // One-to-many with Menu
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();  // One-to-many with Order
        public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();  // One-to-many with Rating

    }

}
