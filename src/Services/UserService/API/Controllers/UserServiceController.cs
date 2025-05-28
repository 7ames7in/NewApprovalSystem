using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;

namespace UserService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserServiceController : ControllerBase
    {
        private readonly IUserRepository<User> _repository;
        private readonly IUserRoleRepository<UserRole> _userRoleRepository;

        public UserServiceController(IUserRepository<User> repository, IUserRoleRepository<UserRole> userRoleRepository)
        {
            _repository = repository;
            this._userRoleRepository = userRoleRepository;
        }

        // Example: Get all users
        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            // Replace with actual logic to retrieve users
            var users = new List<object>
            {
                new { Id = 1, Name = "John Doe", Email = "john.doe@example.com" },
                new { Id = 2, Name = "Jane Smith", Email = "jane.smith@example.com" }
            };

            return Ok(users);
        }

        [HttpGet("search")]
        public IActionResult SearchUsers([FromQuery] string query)
        {
            // Replace with actual logic to search users
            if (string.IsNullOrWhiteSpace(query))
            {
                return new JsonResult(new List<object>());
            }

            var list = this._repository.SearchUsersAsync(query, 1, 10);


            return new JsonResult(list);
        }


        // Example: Get user by ID
        [HttpGet("users/{id}")]
        public IActionResult GetUserById(int id)
        {
            // Replace with actual logic to retrieve a user by ID
            var user = new { Id = id, Name = "John Doe", Email = "john.doe@example.com" };

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // Example: Create a new user
        [HttpPost("users")]
        public IActionResult CreateUser([FromBody] object newUser)
        {
            // Replace with actual logic to create a user
            return CreatedAtAction(nameof(GetUserById), new { id = 1 }, newUser);
        }

        // Example: Update a user
        [HttpPut("users/{id}")]
        public IActionResult UpdateUser(int id, [FromBody] object updatedUser)
        {
            // Replace with actual logic to update a user
            return NoContent();
        }

        // Example: Delete a user
        [HttpDelete("users/{id}")]
        public IActionResult DeleteUser(int id)
        {
            // Replace with actual logic to delete a user
            return NoContent();
        }
    }
}