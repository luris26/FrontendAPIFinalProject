using System.Security.Claims;
using FrontendAPIFinalProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerPupusas.ModelDTOs;
using ServerPupusas.Models;

namespace FrontendAPIFinalProject.Controllers
{
    // http://localhost:5073/api/users/users
    // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;

        public UsersController(IUserService userService, ApplicationDbContext context)
        {
            _userService = userService;
            _context = context;
        }

        [HttpGet("users")]
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _userService.GetAllUsers();
        }

        [HttpPut("edit/{id}")]
        public async Task<IActionResult> EditUser(int id, [FromBody] UserCreateDto updatedUser)
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            existingUser.Name = updatedUser.Name ?? existingUser.Name;
            existingUser.Email = updatedUser.Email ?? existingUser.Email;
            existingUser.Role = updatedUser.Role ?? existingUser.Role;
            existingUser.PasswordHash = updatedUser.PasswordHash ?? existingUser.PasswordHash;

            await _context.SaveChangesAsync();

            return Ok(new { Message = "User updated successfully" });
        }


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddUser([FromBody] UserCreateDto newUser)
        {
            if (newUser == null)
                return BadRequest("Invalid data");

            var userEntity = new User
            {
                Name = newUser.Name,
                Email = newUser.Email,
                Role = newUser.Role,
                PasswordHash = newUser.PasswordHash
            };

            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();

            return Ok(userEntity);
        }

    }
}
