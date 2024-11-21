using Microsoft.AspNetCore.Mvc;
using QuickServe.DTOs;
using QuickServe.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickServe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        // Constructor injection of IUserService
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/user
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserReadDTO>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // GET: api/user/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserReadDTO>> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound("User not found.");
            return Ok(user);
        }

        // POST: api/user
        [HttpPost]
        public async Task<ActionResult<UserReadDTO>> AddUser(UserCreateDTO userDTO)
        {
            var createdUser = await _userService.AddUserAsync(userDTO);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.UserID }, createdUser);
        }

        // PUT: api/user/5
        [HttpPut("{id}")]
        public async Task<ActionResult<UserReadDTO>> UpdateUser(int id, UserUpdateDTO userDTO)
        {
            var updatedUser = await _userService.UpdateUserAsync(id, userDTO);
            return Ok(updatedUser);
        }

        // POST: api/user/login
        [HttpPost("login")]
        public async Task<ActionResult<UserReadDTO>> Login(UserLoginDTO loginDTO)
        {
            var user = await _userService.LoginUserAsync(loginDTO);
            return Ok(user);
        }

        // DELETE: api/user/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result) return NotFound("User not found.");
            return NoContent();
        }
    }
}
