using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuickServe.Data;
using QuickServe.Models;
using QuickServe.Repositories.Interfaces;

namespace QuickServe.Repositories.Implementations
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;

        public PaymentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Payment?> GetPaymentByIdAsync(int id)
        {
            return await _context.Payments.FindAsync(id);
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            return await _context.Payments.ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByUserIdAsync(int userId)
        {
            return await _context.Payments
                .Where(p => p.UserID == userId)  // Query by UserID
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByOrderIdAsync(int orderId)
        {
            return await _context.Payments
                .Where(p => p.OrderID == orderId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(string status)
        {
            return await _context.Payments
                .Where(p => p.PaymentStatus == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Payments
                .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate)
                .ToListAsync();
        }

        public async Task<Payment> AddPaymentAsync(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<Payment> UpdatePaymentAsync(Payment payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<bool> DeletePaymentAsync(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null) return false;

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return true;
        }

        // New method to filter payments by PaymentMethod
        public async Task<IEnumerable<Payment>> GetPaymentsByPaymentMethodAsync(PaymentMethodEnum paymentMethod)
        {
            return await _context.Payments
                .Where(p => p.PaymentMethod == paymentMethod)
                .ToListAsync();
        }
    }
}