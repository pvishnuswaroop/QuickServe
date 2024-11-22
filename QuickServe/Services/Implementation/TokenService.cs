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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using QuickServe.Data;
using QuickServe.Services.Interfaces;

namespace QuickServe.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _jwtSecretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TokenService> _logger;

        // Constructor to inject dependencies and configuration
        public TokenService(IConfiguration configuration, AppDbContext context, ILogger<TokenService> logger)
        {
            _configuration = configuration;
            _context = context;
            _logger = logger;

            // Load JWT settings from appsettings.json
            _jwtSecretKey = _configuration["Jwt:Key"];
            _issuer = _configuration["Jwt:Issuer"];
            _audience = _configuration["Jwt:Audience"];
        }

        // Generate access token (returns a JWT token)
        public async Task<string> GenerateAccessToken(User user)
        {
            return await GenerateJwtToken(user);  // Delegate to GenerateJwtToken
        }

        // Generate JWT token for the user
        public async Task<string> GenerateJwtToken(User user)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecretKey));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                // Add roles to claims if the user has roles
                if (user.Roles != null && user.Roles.Any())
                {
                    foreach (var role in user.Roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }
                }

                // Get expiration time from the configuration (default to 60 minutes if not set)
                var expirationMinutes = _configuration.GetValue<int>("Jwt:ExpirationMinutes", 60);

                var token = new JwtSecurityToken(
                    issuer: _issuer,
                    audience: _audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(expirationMinutes),
                    signingCredentials: credentials
                );

                // Return JWT token
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating JWT token.");
                throw new ApplicationException("An error occurred while generating the token.", ex);
            }
        }

        // Generate a refresh token
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
            try
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(token))
                {
                    throw new ArgumentException("Username or token cannot be null or empty.");
                }

                var refreshToken = new RefreshToken
                {
                    Username = username,
                    Token = token,
                    ExpiryDate = DateTime.UtcNow.AddDays(7)
                };

                await _context.RefreshTokens.AddAsync(refreshToken);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error occurred while saving refresh token.");
                _logger.LogError(dbEx.InnerException?.Message); // Log the inner exception
                throw new ApplicationException("An error occurred while saving the refresh token.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving refresh token.");
                throw new ApplicationException("An error occurred while saving the refresh token.", ex);
            }
        }

        // Retrieve the username associated with the refresh token
        public async Task<string> RetrieveUsernameByRefreshToken(string refreshToken)
        {
            try
            {
                if (string.IsNullOrEmpty(refreshToken))
                {
                    throw new ArgumentNullException(nameof(refreshToken), "Refresh token cannot be null or empty.");
                }

                var tokenRecord = await _context.RefreshTokens
                    .FirstOrDefaultAsync(rt => rt.Token == refreshToken && rt.ExpiryDate > DateTime.UtcNow);

                return tokenRecord?.Username;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving username by refresh token.");
                throw new ApplicationException("An error occurred while retrieving the username by refresh token.", ex);
            }
        }

        // Revoke (delete) the refresh token from the database
        public async Task<bool> RevokeRefreshToken(string refreshToken)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error revoking refresh token.");
                throw new ApplicationException("An error occurred while revoking the refresh token.", ex);
            }
        }
    }
}
