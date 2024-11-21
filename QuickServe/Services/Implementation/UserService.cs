using QuickServe.Models;
using QuickServe.Repositories.Interfaces;
using QuickServe.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickServe.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly ITokenService _tokenService; // Assuming a separate service for generating JWT tokens

        public UserService(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = new PasswordHasher<User>();
            _tokenService = tokenService;
        }

        // Register a new user
        public async Task<User> RegisterUserAsync(string email, string password)
        {
            // Check if the email already exists
            var existingUser = await _userRepository.GetUserByEmailAsync(email);
            if (existingUser != null)
                throw new Exception("User with this email already exists.");

            // Hash the password
            var hashedPassword = _passwordHasher.HashPassword(null, password);

            // Create the user object
            var user = new User
            {
                Email = email,
                PasswordHash = hashedPassword,
                // You can add more user properties here (like Name, Role, etc.)
            };

            // Save user to the database
            return await _userRepository.AddUserAsync(user);
        }

        // Login the user (authenticate)
        public async Task<string> LoginUserAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == null)
                return null; // User not found

            // Verify password
            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
                return null; // Invalid password

            // Generate JWT token using a separate service
            var token = _tokenService.GenerateJwtToken(user); // Assuming a method for generating the JWT

            return token;
        }

        // Get user by ID
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        // Get user by email
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }

        // Get all users (admin functionality)
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        // Update user information
        public async Task<User> UpdateUserAsync(User user)
        {
            // Update user in the database
            return await _userRepository.UpdateUserAsync(user);
        }

        // Delete user
        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _userRepository.DeleteUserAsync(id);
        }
    }
}
