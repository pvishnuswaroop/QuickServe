using Microsoft.AspNetCore.Mvc;
using QuickServe.Services.Interfaces;
using QuickServe.DTO;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using QuickServe.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuickServe.Controllers
{
    namespace QuickServe.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class AuthController : ControllerBase
        {
            private readonly IUserService _userService;
            private readonly ITokenService _tokenService;
            private readonly PasswordHasher<User> _passwordHasher;
            private readonly ILogger<AuthController> _logger;

            public AuthController(IUserService userService, ITokenService tokenService, ILogger<AuthController> logger)
            {
                _userService = userService;
                _tokenService = tokenService;
                _passwordHasher = new PasswordHasher<User>();
                _logger = logger;
            }

            [HttpPost("Register")]
            public async Task<IActionResult> Register([FromBody] Register request)
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid request body.");

                try
                {
                    // Hash the password before saving
                    var hashedPassword = _passwordHasher.HashPassword(new User(), request.Password);

                    // Call the RegisterUserAsync method with email, hashed password, and role
                    var userDto = await _userService.RegisterUserAsync(request.Email, hashedPassword, request.Role);

                    // Map the returned UserDto to a User entity
                    var user = new User
                    {
                        Email = userDto.Email,
                        Name = userDto.Name,
                        ContactNumber = userDto.ContactNumber
                    };

                    // Generate an access token for the registered user
                    var accessToken = _tokenService.GenerateAccessToken(user);

                    return Ok(new { AccessToken = accessToken });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred during user registration.");
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }

            [HttpPost("Login")]
            public async Task<IActionResult> Login([FromBody] Login request)
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid request: " + string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                    return BadRequest("Invalid request body.");
                }

                try
                {
                    var userDto = await _userService.GetUserByEmailAsync(request.Email);
                    if (userDto == null)
                        return Unauthorized("Invalid user credentials.");

                    // Log the password hash to verify it's being retrieved correctly
                    _logger.LogInformation($"Retrieved PasswordHash for {request.Email}: {userDto.PasswordHash}");

                    // Check if password hash exists
                    if (string.IsNullOrEmpty(userDto.PasswordHash))
                        return Unauthorized("Password hash not found.");

                    var user = new User
                    {
                        UserID = userDto.Id,
                        Email = userDto.Email,
                        Name = userDto.Name,
                        ContactNumber = userDto.ContactNumber,
                        PasswordHash = userDto.PasswordHash
                    };

                    if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password) != PasswordVerificationResult.Success)
                        return Unauthorized("Invalid user credentials.");

                    var accessToken = await _tokenService.GenerateAccessToken(user);
                    var refreshToken = _tokenService.GenerateRefreshToken();

                    await _tokenService.SaveRefreshToken(user.Email, refreshToken);

                    return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred during login.");
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }



            [HttpPost("RefreshToken")]
            public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
            {
                if (request == null || string.IsNullOrWhiteSpace(request.RefreshToken))
                    return BadRequest("Refresh token is required.");

                try
                {
                    var username = await _tokenService.RetrieveUsernameByRefreshToken(request.RefreshToken);
                    if (string.IsNullOrEmpty(username))
                        return Unauthorized("Invalid refresh token.");

                    var userDto = await _userService.GetUserByEmailAsync(username);
                    if (userDto == null)
                        return Unauthorized("Invalid user.");

                    var user = new User
                    {
                        UserID = userDto.Id,
                        Email = userDto.Email,
                        Name = userDto.Name,
                        ContactNumber = userDto.ContactNumber,
                        PasswordHash = userDto.PasswordHash
                    };

                    var accessToken = await _tokenService.GenerateAccessToken(user);
                    var newRefreshToken = _tokenService.GenerateRefreshToken();
                    await _tokenService.SaveRefreshToken(user.Email, newRefreshToken);

                    return Ok(new { AccessToken = accessToken, RefreshToken = newRefreshToken });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred during token refresh.");
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }

            [HttpPost("RevokeToken")]
            public async Task<IActionResult> RevokeToken([FromBody] RefreshTokenRequest request)
            {
                if (request == null || string.IsNullOrWhiteSpace(request.RefreshToken))
                    return BadRequest("Refresh token is required.");

                try
                {
                    var result = await _tokenService.RevokeRefreshToken(request.RefreshToken);
                    if (!result)
                        return NotFound("Refresh token not found.");

                    return Ok("Token revoked.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred during token revocation.");
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
        }
    }
}

