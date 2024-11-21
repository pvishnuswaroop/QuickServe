using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuickServe.Data;
using QuickServe.Models;
using QuickServe.Repositories.Interfaces;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace QuickServe.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserRepository> _logger;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserRepository(AppDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserID == id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.AsNoTracking().ToListAsync();
        }

        public async Task<User> AddUserAsync(User user)
        {
            user.PasswordHash = _passwordHasher.HashPassword(user, user.PasswordHash); // Hash the password
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

            // Update fields if they are provided
            existingUser.Name = user.Name ?? existingUser.Name;
            existingUser.Gender = user.Gender ?? existingUser.Gender;
            existingUser.ContactNumber = user.ContactNumber ?? existingUser.ContactNumber;
            existingUser.Email = user.Email ?? existingUser.Email;
            existingUser.Address = user.Address ?? existingUser.Address;

            // Update password if provided
            if (!string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                existingUser.PasswordHash = _passwordHasher.HashPassword(existingUser, user.PasswordHash);
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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            if (user == null || !user.ValidatePassword(password))
            {
                _logger.LogWarning("Login failed for email: {Email}", email);
                throw new AuthenticationException("Invalid email or password.");
            }

            // Update last login time
            user.LastLogin = DateTime.Now;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User {Email} logged in successfully.", email);
            return user;
        }
    }
}
