using System.Collections.Generic;
using System.Threading.Tasks;
using QuickServe.Models;

namespace QuickServe.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment?> GetPaymentByIdAsync(int id);  // Use nullable to indicate the payment might not exist
        Task<IEnumerable<Payment>> GetAllPaymentsAsync();
        Task<IEnumerable<Payment>> GetPaymentsByUserIdAsync(int userId);  // Optional: Get payments by user ID
        Task<IEnumerable<Payment>> GetPaymentsByOrderIdAsync(int orderId);  // Optional: Get payments by order ID
        Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(string status);  // Optional: Get payments by status
        Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate);  // Optional: Get payments within a date range
        Task<Payment> AddPaymentAsync(Payment payment);
        Task<Payment> UpdatePaymentAsync(Payment payment);
        Task<bool> DeletePaymentAsync(int id);
    }
}
