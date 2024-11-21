using Microsoft.EntityFrameworkCore;
using QuickServe.Models;
using QuickServe.Data;
using QuickServe.Repositories.Interfaces;
using System.Threading.Tasks;
using System.Security.Authentication;

namespace QuickServe.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(AppDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Implement GetUserByEmailAsync
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);  // Retrieve user by email
        }

        // Other methods...

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserID == id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            return user;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.AsNoTracking().ToListAsync();
        }

        public async Task<User> AddUserAsync(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Password))
            {
                throw new ArgumentException("Password cannot be null or empty.");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.UserID);
            if (existingUser == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            existingUser.Name = user.Name ?? existingUser.Name;
            existingUser.Gender = user.Gender ?? existingUser.Gender;
            existingUser.ContactNumber = user.ContactNumber ?? existingUser.ContactNumber;
            existingUser.Email = user.Email ?? existingUser.Email;
            existingUser.Address = user.Address ?? existingUser.Address;

            if (!string.IsNullOrWhiteSpace(user.Password))
            {
                existingUser.Password = user.Password;  // Update password directly (plaintext)
            }

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();
            return existingUser;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User> LoginUserAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || !user.ValidatePassword(password))
            {
                _logger.LogWarning("Login failed for email: {Email}", email);
                throw new AuthenticationException("Invalid email or password.");
            }

            _logger.LogInformation("User {Email} logged in successfully.", email);
            return user;
        }
    }
}
