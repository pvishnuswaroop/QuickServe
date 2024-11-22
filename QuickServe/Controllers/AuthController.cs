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
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
            _passwordHasher = new PasswordHasher<User>();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] Register request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid request body.");

            try
            {
                // Assuming RegisterUserAsync returns a UserDto
                var hashedPassword = _passwordHasher.HashPassword(new User(), request.Password);
                var userDto = await _userService.RegisterUserAsync(request.Email, hashedPassword);


                // Create the User model from UserDto
                var user = new User
                {
                    Email = userDto.Email,
                    Name = userDto.Name,
                    ContactNumber = userDto.ContactNumber
                    // Do not assign PasswordHash, it's already handled in RegisterUserAsync
                };

                // Now you can pass the User model to the relevant method
                var accessToken = _tokenService.GenerateAccessToken(user);

                return Ok(new { AccessToken = accessToken });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Login request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid request body.");
            }

            try
            {
                // Assuming GetUserByEmailAsync returns a UserDto
                var userDto = await _userService.GetUserByEmailAsync(request.Email);

                if (userDto == null)
                {
                    return Unauthorized("Invalid user credentials.");
                }

                // Manually map UserDto to User (or directly modify GetUserByEmailAsync to return a User)
                var user = new User
                {
                    UserID = userDto.Id,
                    Email = userDto.Email,
                    Name = userDto.Name,
                    ContactNumber = userDto.ContactNumber,
                    PasswordHash = userDto.PasswordHash // If password hash is already stored in the DTO
                };

                // Verify the password using the hashed password
                if (!_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password).Equals(PasswordVerificationResult.Success))
                {
                    return Unauthorized("Invalid user credentials.");
                }

                // Issue access and refresh tokens
                var accessToken = _tokenService.GenerateAccessToken(user); // Now passing the User model
                var refreshToken = _tokenService.GenerateRefreshToken();

                // Save refresh token for the user
                await _tokenService.SaveRefreshToken(user.Email, refreshToken);

                return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
            }
            catch (Exception ex)
            {
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

                // Assuming GetUserByEmailAsync returns a UserDto
                var userDto = await _userService.GetUserByEmailAsync(username);
                if (userDto == null)
                    return Unauthorized("Invalid user.");

                // Map UserDto to User model
                var user = new User
                {
                    UserID = userDto.Id,
                    Email = userDto.Email,
                    Name = userDto.Name,
                    ContactNumber = userDto.ContactNumber,
                    PasswordHash = userDto.PasswordHash // If PasswordHash is part of the DTO
                };

                // Generate new access token and refresh token
                var accessToken = _tokenService.GenerateAccessToken(user); // Now passing the User model
                var newRefreshToken = _tokenService.GenerateRefreshToken();
                await _tokenService.SaveRefreshToken(user.Email, newRefreshToken);

                return Ok(new { AccessToken = accessToken, RefreshToken = newRefreshToken });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        // Revoke the refresh token
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
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
