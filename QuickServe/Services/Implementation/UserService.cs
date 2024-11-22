using QuickServe.Models;
using QuickServe.Repositories.Interfaces;
using QuickServe.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuickServe.DTO;
using Microsoft.EntityFrameworkCore;

namespace QuickServe.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly ITokenService _tokenService;

        // Constructor injects the necessary services
        public UserService(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = new PasswordHasher<User>();
            _tokenService = tokenService;
        }

        // Register a new user
        public async Task<UserDto> RegisterUserAsync(string email, string password, string role = "Customer")
        {
            // Check if the user already exists
            var existingUser = await _userRepository.GetUserByEmailAsync(email);
            if (existingUser != null)
                throw new Exception("User with this email already exists.");

            // Hash the password
            var hashedPassword = _passwordHasher.HashPassword(new User(), password);

            // Create a new User object and set the hashed password
            var user = new User
            {
                Email = email,
                PasswordHash = _passwordHasher.HashPassword(null, password),   // Correctly set the hashed password
                Name = "Default Name",          // Default name, can be customized
                ContactNumber = "0000000000",   // Default number, can be updated later
                Address = "Default Address",    // Default address
                CreatedAt = DateTime.Now,       // Set the creation timestamp
                Role = role,                    // Role passed to the method
                Roles = new List<string> { role } // Set roles for the user
            };

            // Save the user using the repository
            await _userRepository.AddUserAsync(user);

            // Return the user data (UserDto)
            return new UserDto
            {
                Id = user.UserID,
                Email = user.Email,
                Name = user.Name,
                ContactNumber = user.ContactNumber
            };
        }


        // Login the user (authenticate)
        public async Task<string> LoginUserAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == null)
                return null;

            // Log to check the PasswordHash value
            Console.WriteLine($"Password Hash: {user.PasswordHash}");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (result == PasswordVerificationResult.Failed)
                return null;

            return await _tokenService.GenerateJwtToken(user);
        }


        // Get user by ID
        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                throw new Exception("User not found");

            return new UserDto
            {
                Id = user.UserID,
                Email = user.Email,
                Name = user.Name,
                ContactNumber = user.ContactNumber
            };
        }

        // Get user by email
        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
                return null;

            return new UserDto
            {
                Id = user.UserID,
                Email = user.Email,
                Name = user.Name,
                ContactNumber = user.ContactNumber
            };
        }

        // Get all users (admin functionality)
        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();

            return users.Select(user => new UserDto
            {
                Id = user.UserID,
                Email = user.Email,
                Name = user.Name,
                ContactNumber = user.ContactNumber
            });
        }

        // Update user information
        public async Task<UserDto> UpdateUserAsync(User user)
        {
            var updatedUser = await _userRepository.UpdateUserAsync(user);

            return new UserDto
            {
                Id = updatedUser.UserID,
                Email = updatedUser.Email,
                Name = updatedUser.Name,
                ContactNumber = updatedUser.ContactNumber
            };
        }

        // Delete user
        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _userRepository.DeleteUserAsync(id);
        }
    }
}
