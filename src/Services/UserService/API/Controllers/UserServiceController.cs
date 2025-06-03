using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;
using UserService.API.Dtos;

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
            _userRoleRepository = userRoleRepository;
        }

        /// <summary>
        /// Handles user login attempts.
        /// </summary>
        /// <param name="user">Login details provided by the user.</param>
        /// <returns>Returns login success or failure response.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto user)
        {
            var userinfo = await _repository.ValidateLoginAndInfomationAsync(user.EmailId);

            if (userinfo != null && userinfo.Email == user.EmailId)
            {
                return Ok(new
                {
                    Message = "Login successful",
                    EmployeeNumber = userinfo?.EmployeeNumber,
                    Name = userinfo?.Name,
                    Email = userinfo?.Email,
                    Department = userinfo?.Department,
                    Position = userinfo?.Position,
                    Role = userinfo?.Role
                });
            }

            return Unauthorized(new { Message = "Invalid login attempt" });
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>Returns a list of users.</returns>
        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            var users = new List<object>
            {
                new { Id = 1, Name = "John Doe", Email = "john.doe@example.com" },
                new { Id = 2, Name = "Jane Smith", Email = "jane.smith@example.com" }
            };

            return Ok(users);
        }

        /// <summary>
        /// Searches for users based on a query string.
        /// </summary>
        /// <param name="query">Search query string.</param>
        /// <returns>Returns a list of matching users.</returns>
        [HttpGet("search")]
        public async Task<IActionResult> SearchUsers([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return new JsonResult(new List<object>());
            }

            var list = await _repository.SearchUsersAsync(query, 1, 10);

            return new JsonResult(list);
        }

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        /// <param name="id">User ID.</param>
        /// <returns>Returns the user details or a not found response.</returns>
        [HttpGet("users/{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = new { Id = id, Name = "John Doe", Email = "john.doe@example.com" };

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="newUser">Details of the new user.</param>
        /// <returns>Returns the created user details.</returns>
        [HttpPost("users")]
        public IActionResult CreateUser([FromBody] object newUser)
        {
            return CreatedAtAction(nameof(GetUserById), new { id = 1 }, newUser);
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id">User ID.</param>
        /// <param name="updatedUser">Updated user details.</param>
        /// <returns>Returns no content response.</returns>
        [HttpPut("users/{id}")]
        public IActionResult UpdateUser(int id, [FromBody] object updatedUser)
        {
            return NoContent();
        }

        /// <summary>
        /// Deletes a user by their ID.
        /// </summary>
        /// <param name="id">User ID.</param>
        /// <returns>Returns no content response.</returns>
        [HttpDelete("users/{id}")]
        public IActionResult DeleteUser(int id)
        {
            return NoContent();
        }
    }
}