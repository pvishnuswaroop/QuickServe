using System;
using System.ComponentModel.DataAnnotations;

namespace QuickServe.Models
{
    public class Cart
    {
        [Key]
        public int CartID { get; set; }

        [Required]
        public int UserID { get; set; }  // Foreign Key

        [Required]
        public DateTime CreationDate { get; set; }

        // Constructor
        public Cart(int userID, DateTime creationDate)
        {
            UserID = userID;
            CreationDate = creationDate;
        }
    }
}
