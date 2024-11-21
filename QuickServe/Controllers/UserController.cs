using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickServe.Services.Interfaces; // If using services for logic
using QuickServe.Data;  // For accessing the database context
using QuickServe.Models; // Your models namespace

namespace QuickServe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context; // Injecting the DbContext

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            // Retrieve all users from the database
            var users = await _context.Users.ToListAsync();
            return Ok(users); // Return the list of users as a response
        }

        // GET api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            // Retrieve a specific user by ID
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound(); // Return 404 if the user is not found
            }

            return Ok(user); // Return the user data
        }
    }
}
