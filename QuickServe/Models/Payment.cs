using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickServe.Models
{
    public enum PaymentMethodEnum
    {
        CreditCard,
        PayPal,
        Cash,
        BankTransfer,
        Other
    }

    public class Payment
    {
        [Key]
        public int PaymentID { get; set; }

        [Required(ErrorMessage = "Order ID is required.")]
        [ForeignKey("Order")]
        public int OrderID { get; set; }  // Foreign Key for Order

        [Required(ErrorMessage = "Amount paid is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount paid must be greater than zero.")]
        public decimal AmountPaid { get; set; }

        [Required(ErrorMessage = "Payment status is required.")]
        [StringLength(50, ErrorMessage = "Payment status cannot exceed 50 characters.")]
        public string PaymentStatus { get; set; }  // Payment status (e.g., "Completed", "Pending")

        [Required(ErrorMessage = "Payment date is required.")]
        public DateTime PaymentDate { get; set; }

        [Required(ErrorMessage = "Payment method is required.")]
        public PaymentMethodEnum PaymentMethod { get; set; }  // Enum for payment method

        // Navigation properties
        public virtual Order Order { get; set; }
        public virtual User User { get; set; }  // Navigation property to User

        // Optional: If you want to track the user making the payment, add a foreign key
        [Required(ErrorMessage = "User ID is required.")]
        [ForeignKey("User")]
        public int UserID { get; set; }  // Foreign Key for User (optional, based on your use case)
    }
}
