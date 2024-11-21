using QuickServe.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using QuickServe.Services.Interfaces;
using Microsoft.Extensions.Configuration;  // Required for IConfiguration
using QuickServe.Data;

namespace QuickServe.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _jwtSecretKey;  // Secret key for signing JWT tokens
        private readonly string _issuer;       // Issuer of the JWT token
        private readonly string _audience;     // Audience for the JWT token
        private readonly AppDbContext _context; // DB context for saving refresh tokens
        private readonly IConfiguration _configuration; // To access appsettings.json

        // Constructor to inject the IConfiguration and AppDbContext
        public TokenService(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;

            // Load JWT settings from appsettings.json
            _jwtSecretKey = _configuration["Jwt:Key"];  // Secret Key from appsettings
            _issuer = _configuration["Jwt:Issuer"];     // Issuer from appsettings
            _audience = _configuration["Jwt:Audience"]; // Audience from appsettings
        }

        // Method to generate a JWT token
        public string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString())
            };

            // Add roles to claims if they exist
            foreach (var role in user.Roles)  // Iterate over the string list directly
            {
                claims.Add(new Claim(ClaimTypes.Role, role));  // Use the role (which is a string)
            }

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),  // Token expiry
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Method to generate a refresh token
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }
            return Convert.ToBase64String(randomNumber);
        }

        // Save refresh token to the database
        public async Task SaveRefreshToken(string username, string token)
        {
            var refreshToken = new RefreshToken
            {
                Username = username,
                Token = token,
                ExpiryDate = DateTime.UtcNow.AddDays(7)  // Set the expiration for the refresh token
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
        }

        // Retrieve the username associated with a given refresh token
        public async Task<string> RetrieveUsernameByRefreshToken(string refreshToken)
        {
            var tokenRecord = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken && rt.ExpiryDate > DateTime.UtcNow);

            return tokenRecord?.Username;
        }

        // Revoke (delete) the refresh token
        public async Task<bool> RevokeRefreshToken(string refreshToken)
        {
            var tokenRecord = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (tokenRecord != null)
            {
                _context.RefreshTokens.Remove(tokenRecord);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
