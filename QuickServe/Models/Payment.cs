using System;
using System.ComponentModel.DataAnnotations;

namespace QuickServe.Models
{
    public class Payment
    {
        [Key]
        public int PaymentID { get; set; }

        [Required]
        public int OrderID { get; set; }  // Foreign Key

        [Required]
        public decimal AmountPaid { get; set; }

        [Required]
        public string PaymentStatus { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        // Navigation property
        public virtual Order? Order { get; set; }  // One-to-one with Order

        // Constructor
        public Payment(int orderID, decimal amountPaid, string paymentStatus, DateTime paymentDate)
        {
            OrderID = orderID;
            AmountPaid = amountPaid;
            PaymentStatus = paymentStatus;
            PaymentDate = paymentDate;
        }
    }

}
