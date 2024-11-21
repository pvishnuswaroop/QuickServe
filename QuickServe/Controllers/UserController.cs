using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickServe.DTO;
using QuickServe.DTOs;
using QuickServe.Models;
using QuickServe.Services;
using QuickServe.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickServe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // POST: api/user/register
        [HttpPost("register")]
        public async Task<ActionResult<User>> RegisterUser([FromBody] RegisterDto registerDto)
        {
            if (registerDto == null)
                return BadRequest("Invalid user data.");

            var registeredUser = await _userService.RegisterUserAsync(registerDto.Email, registerDto.Password);

            return CreatedAtAction(nameof(GetUserById), new { id = registeredUser.UserID }, registeredUser);
        }


        // POST: api/user/login
        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginUser([FromBody] LoginDto loginDto)
        {
            if (loginDto == null)
                return BadRequest("Invalid credentials.");

            var token = await _userService.LoginUserAsync(loginDto.Email, loginDto.Password);

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Invalid email or password.");

            return Ok(new { Token = token });
        }

        // GET: api/user/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            if (updateUserDto == null)
                return BadRequest("Invalid user data.");

            var existingUser = await _userService.GetUserByIdAsync(id);
            if (existingUser == null)
                return NotFound();

            existingUser.Name = updateUserDto.Name;
            existingUser.ContactNumber = updateUserDto.ContactNumber;
            existingUser.Address = updateUserDto.Address;

            await _userService.UpdateUserAsync(existingUser);
            return NoContent();
        }


        // DELETE: api/user/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            await _userService.DeleteUserAsync(id);
            return NoContent();
        }

        // GET: api/user
        [HttpGet]
        [Authorize(Roles = "Admin")] // Only Admins can view all users
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
    }
}
