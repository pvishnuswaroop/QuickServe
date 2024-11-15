using System;
using System.ComponentModel.DataAnnotations;

namespace QuickServe.Models
{
    public class Restaurant
    {
        [Key]
        public int RestaurantID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string ContactNumber { get; set; }

        // Constructor
        public Restaurant(string name, string location, string contactNumber)
        {
            Name = name;
            Location = location;
            ContactNumber = contactNumber;
        }
    }
}
