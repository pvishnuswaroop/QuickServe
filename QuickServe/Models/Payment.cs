using System;
using System.ComponentModel.DataAnnotations;

namespace QuickServe.Models
{
    public class Payment
    {
        [Key]
        public int PaymentID { get; set; }

        [Required(ErrorMessage = "Order ID is required.")]
        public int OrderID { get; set; }  // Foreign Key

        [Required(ErrorMessage = "Amount paid is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount paid must be greater than zero.")]
        public decimal AmountPaid { get; set; }

        [Required(ErrorMessage = "Payment status is required.")]
        [StringLength(50, ErrorMessage = "Payment status cannot exceed 50 characters.")]
        public string? PaymentStatus { get; set; }

        [Required(ErrorMessage = "Payment date is required.")]
        public DateTime PaymentDate { get; set; }

        // New attribute for payment method
        [Required(ErrorMessage = "Payment method is required.")]
        [StringLength(50, ErrorMessage = "Payment method cannot exceed 50 characters.")]
        public string? PaymentMethod { get; set; }  // E.g., "Credit Card", "PayPal", "Cash", etc.

        // Navigation property
        public virtual Order? Order { get; set; }  // One-to-one with Order
    }
}
